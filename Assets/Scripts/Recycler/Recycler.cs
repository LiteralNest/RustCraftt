using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory_System;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Recycler : Storage
{
    public NetworkVariable<bool> Turned = new NetworkVariable<bool>(false);

    [Header("Main Params")] [SerializeField]
    private float _recyclingTime = 1;

    [Header("Cells")] [SerializeField] private List<RecyclingItem> _avaliableItems = new List<RecyclingItem>();

    [Header("Audio")] [SerializeField] private AudioSource _source;

    private bool _recycling;
 
    private void Start()
        => gameObject.tag = "Recycler";

    private void Update()
    {
        if (!Turned.Value) return;
        TryRecycle();
    }

    public override void Open(InventoryHandler handler)
    {
        handler.InventoryPanelsDisplayer.OpenRecyclerPanel();
        SlotsDisplayer = handler.RecyclerSlotsDisplayer;
        base.Open(handler);
    }

    private RecyclingItem GetRecyclingItemById(int id)
    {
        foreach (var item in _avaliableItems)
        {
            if (item.Id == id)
                return item;
        }

        return null;
    }

    private async void RecycleItem(RecyclingItem item)
    {
        await Task.Delay((int)(_recyclingTime * 1000));
        if (!_recycling) return;
        List<CustomSendingInventoryDataCell> recyclingCells = new List<CustomSendingInventoryDataCell>();
        for (int i = 5; i < 10; i++)
        {
            recyclingCells.Add(new CustomSendingInventoryDataCell(ItemsNetData.Value.Cells[i]));
        }

        foreach (var cell in item.Cells)
        {
            var rand = Random.Range(cell.ItemsRange.x, cell.ItemsRange.y);
            var desiredCellId = 5 + InventoryHelper.GetDesiredCellId(cell.ResultItem.Id, rand, ItemsNetData);
            if (desiredCellId == -1)
            {
                _recycling = false;
                return;
            }
            SetItemServerRpc(desiredCellId, new CustomSendingInventoryDataCell(cell.ResultItem.Id, rand, -1));
        }

        RemoveItem(item, 1);
        _recycling = false;
    }

    private void TryRecycle()
    {
        if (!IsServer) return;
        if (_recycling) return;
        List<CustomSendingInventoryDataCell> cells = new List<CustomSendingInventoryDataCell>();
        for (int i = 0; i < 5; i++)
        {
            cells.Add(new CustomSendingInventoryDataCell(ItemsNetData.Value.Cells[i]));
        }
        foreach (var cell in cells)
        {
            if (cell.Id == -1 || !GetRecyclingItemById(cell.Id)) continue;
            RecycleItem(GetRecyclingItemById(cell.Id));
            _recycling = true;
            return;
        }

        SetTurnedServerRpc(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetTurnedServerRpc(bool value)
    {
        if (IsServer)
        {
            Turned.Value = value;
        }

        _source.Play();
        if (!value)
        {
            _source.Stop();
            _recycling = value;
        }
    }
}