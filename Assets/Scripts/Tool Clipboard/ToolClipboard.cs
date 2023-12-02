using System.Collections.Generic;
using AuthorizationSystem;
using Building_System.Blocks;
using Inventory_System.Slots_Displayer.Tool_CLipBoard;
using Lock_System;
using Multiplayer.CustomData;
using Storage_System;
using Unity.Netcode;
using UnityEngine;
using Web.User;

public class ToolClipboard : Storage, ILockable
{
    [SerializeField] private Transform _mainTransform;
    private Locker _targetLocker;

    [SerializeField] private List<BuildingBlock> _connectedBlocks = new List<BuildingBlock>();
    public List<BuildingBlock> ConnectedBlocks => _connectedBlocks;
    [SerializeField] private NetworkVariable<AuthorizedUsersData> _authorizedIds = new();
    private bool _isLocked;

    public override void Open(InventoryHandler handler)
    {
        if (_targetLocker != null && !_targetLocker.CanBeOpened(UserDataHandler.singleton.UserData.Id))
        {
            _targetLocker.Open();
            return;
        }
        SlotsDisplayer = handler.ToolClipboardSlotsDisplayer;
        handler.InventoryPanelsDisplayer.OpenClipBoardPanel();
        base.Open(handler);
    }

    protected override void SetItem(int cellId, CustomSendingInventoryDataCell dataCell)
    {
        base.SetItem(cellId, dataCell);
        var slotsDisplayer = SlotsDisplayer as ToolClipBoardSlotsDisplayer;
        slotsDisplayer.DisplayRemainingTime();
    }


    #region Counting Remaining Time

    private List<InventoryCell> GetConvertedItemsList(List<CustomSendingInventoryDataCell> data)
    {
        var res = new List<InventoryCell>();
        foreach (var cell in data)
        {
            if (cell.Id == -1) continue;
            res.Add(new InventoryCell(ItemFinder.singleton.GetItemById(cell.Id), cell.Count));
        }

        return res;
    }

    private bool TryMinusItemsPreSecond(List<InventoryCell> delCells,
        List<InventoryCell> data)
    {
        var deletingCells = new List<InventoryCell>(delCells);
        for (int i = 0; i < deletingCells.Count; i++)
        {
            for (int j = 0; j < data.Count; j++)
            {
                if (deletingCells[i].Item == null || data[j].Item == null) continue;
                if (deletingCells[i].Item.Id == data[j].Item.Id)
                {
                    if (deletingCells.Count <= data[j].Count)
                    {
                        if (data[j].Count <= 0)
                        {
                            data.RemoveAt(j);
                            j--;
                        }
                        else
                            data[j].Count -= deletingCells.Count;

                        deletingCells.RemoveAt(i);
                        i--;
                        if (deletingCells.Count == 0) return true;
                        continue;
                    }

                    deletingCells[i].Count -= data[j].Count;
                    if (deletingCells.Count == 0) return true;
                }
            }

            if (deletingCells.Count == 0) return true;
        }

        return false;
    }

    private List<InventoryCell> GetRemovingCells()
    {
        List<InventoryCell> res = new List<InventoryCell>();
        foreach (var block in _connectedBlocks)
        {
            foreach (var cell in block.CurrentBlock.CellsForRemovingPerTime)
                res.Add(cell);
        }

        return res;
    }

    private List<InventoryCell> GetClipBoardCells()
    {
        List<CustomSendingInventoryDataCell> cells = new List<CustomSendingInventoryDataCell>(ItemsNetData.Value.Cells);
        return GetConvertedItemsList(cells);
    }

    public int GetAvaliableMinutes()
    {
        var res = 0;
        List<InventoryCell> deletingCells = GetRemovingCells();
        var list = GetClipBoardCells();
        while (TryMinusItemsPreSecond(deletingCells, list))
            res++;
        return res;
    }

    private void AddSlotToInventory(InventoryCell addingCell, List<InventoryCell> data)
    {
        foreach (var slot in data)
        {
            if (slot.Item.Id == addingCell.Item.Id)
            {
                slot.Count += addingCell.Count;
                return;
            }
        }

        data.Add(addingCell);
    }

    private List<InventoryCell> GetStackedList(List<InventoryCell> inputList)
    {
        var res = new List<InventoryCell>();
        if (inputList.Count == 0) return res;
        res.Add(inputList[0]);
        inputList.RemoveAt(0);
        foreach (var cell in inputList)
            AddSlotToInventory(cell, res);
        return res;
    }

    public List<InventoryCell> GetNeededResourcesForDay()
    {
        var res = new List<InventoryCell>();
        var inputList = GetStackedList(GetRemovingCells());
        foreach (var cell in inputList)
            res.Add(new InventoryCell(cell.Item, cell.Count * 24));
        return res;
    }

    #endregion

    #region ILockable

    public void Lock(Locker locker)
        => _targetLocker = locker;
    
    public Transform GetParent()
        => _mainTransform;
    
    public bool IsLocked()
    {
        if(_targetLocker == null) return false;
        return _targetLocker.IsLocked();
    }

    public bool IsAutorized(int value)
    {
        var helper = new AuthorizationHelper();
        return helper.IsAuthorized(value, _authorizedIds);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AuthorizeServerRpc(int id)
    {
        if (_targetLocker != null && !_targetLocker.CanBeOpened(UserDataHandler.singleton.UserData.Id))
        {
            _targetLocker.Open();
            return;
        }
        if (!IsServer) return;
        var helper = new AuthorizationHelper();
        helper.Authorize(id, _authorizedIds);
    }

    #endregion
}