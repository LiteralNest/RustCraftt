using System.Collections.Generic;
using AlertsSystem;
using AuthorizationSystem;
using Inventory_System;
using Lock_System;
using Multiplayer.CustomData;
using Storage_System;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace Tool_Clipboard
{
    public class ToolClipboard : DropableStorage, ILockable
    {
        [Header("UI")] [SerializeField] private GameObject _selectingCircle;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private NetworkVariable<AuthorizedUsersData> _authorizedIds = new();
        [SerializeField] private ShelfZoneHandler _shelfZoneHandler;

        private Locker _targetLocker;

        public AuthorizedUsersData AuthorizedIds => _authorizedIds.Value;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            ItemsNetData.OnValueChanged +=
                (CustomSendingInventoryData prevValue, CustomSendingInventoryData newValue) =>
                {
                    SlotsDisplayer.DisplayCells();
                };
        }

        public bool IsDecay()
        {
            if(_shelfZoneHandler.ConnectedBlocks.Count == 0) return false;
            return GetAvailableHours() <= 0;
        }

        public override void Open(InventoryHandler handler)
        {
            if (_targetLocker != null && !_targetLocker.CanBeOpened(UserDataHandler.Singleton.UserData.Id))
            {
                _targetLocker.Open();
                return;
            }

            _selectingCircle.SetActive(true);
            _inventoryPanel.SetActive(false);
            Appear();
            Ui.SetActive(true);
        }

        public void OpenInventory()
        {
            CurrentInventoriesHandler.Singleton.CurrentStorage = this;
            InventoryHandler.singleton.InventoryPanelsDisplayer.OpenInventory(true);
            SlotsDisplayer.DisplayCells();
        }

        [ServerRpc(RequireOwnership = false)]
        public void ClearAuthorizedIdsServerRpc()
        {
            if (!IsServer) return;
            var data = new int[0];
            _authorizedIds.Value = new AuthorizedUsersData(data);
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

        private List<InventoryCell> GetRemovingCells()
        {
            List<InventoryCell> res = new List<InventoryCell>();
            foreach (var block in _shelfZoneHandler.ConnectedBlocks)
            {
                foreach (var cell in block.CurrentBlock.CellsForRemovingPerTime)
                    res.Add(cell);
            }

            return res;
        }

        private List<InventoryCell> GetClipBoardCells()
        {
            List<CustomSendingInventoryDataCell> cells =
                new List<CustomSendingInventoryDataCell>(ItemsNetData.Value.Cells);
            return GetConvertedItemsList(cells);
        }

        public int GetAvailableHours()
        {
            var res = 0;
            List<InventoryCell> deletingCells = GetStackedList(GetRemovingCells());
            var list = GetStackedList(GetClipBoardCells());

            if (deletingCells.Count == 0) return res;

            while (true)
            {
                foreach (var deleteCell in deletingCells)
                {
                    var cachedDeletingList = new List<InventoryCell>(deletingCells);
                    foreach (var inventoryCell in list)
                    {
                        if (deleteCell.Item.Id == inventoryCell.Item.Id)
                        {
                            if (inventoryCell.Count < deleteCell.Count) return res;
                            res++;
                            cachedDeletingList.Remove(deleteCell);
                            inventoryCell.Count -= deleteCell.Count;
                            break;
                        }
                    }

                    if (cachedDeletingList.Count != 0) return res;
                }
            }

            return res;
        }

        private List<InventoryCell> GetStackedList(List<InventoryCell> inputList)
        {
            var res = new List<InventoryCell>();
            if (inputList.Count == 0) return res;
            foreach (var cell in inputList)
            {
                var addingCell = new InventoryCell(cell);
                bool wasAdded = false;
                foreach (var resCell in res)
                {
                    if (resCell.Item.Id == cell.Item.Id)
                    {
                        resCell.Count += cell.Count;
                        wasAdded = true;
                        break;
                    }
                }

                if (!wasAdded)
                    res.Add(addingCell);
            }

            return res;
        }

        public List<InventoryCell> GetNeededResourcesForHour()
        {
            var res = new List<InventoryCell>();
            var inputList = GetStackedList(GetRemovingCells());
            foreach (var cell in inputList)
                res.Add(new InventoryCell(cell.Item, cell.Count * 24));
            return res;
        }

        #endregion

        #region ILockable

        public bool IsAutorized(int value)
        {
            var helper = new AuthorizationHelper();
            return helper.IsAuthorized(value, _authorizedIds);
        }

        [ServerRpc(RequireOwnership = false)]
        public void AuthorizeServerRpc(int id)
        {
            if (!IsServer) return;
            var helper = new AuthorizationHelper();
            helper.Authorize(id, _authorizedIds);
        }

        #endregion

        private bool CanBeOpened()
            => !(_targetLocker != null && !_targetLocker.CanBeOpened(UserDataHandler.Singleton.UserData.Id));
        
        public override string GetDisplayText()
        {
            if(!CanBeOpened()) return "Locked";
            if (!IsAutorized(UserDataHandler.Singleton.UserData.Id)) return "Authorize";
            return base.GetDisplayText();
        }

        public override void Interact()
        {
            if (!IsAutorized(UserDataHandler.Singleton.UserData.Id))
            {
                if (_targetLocker != null && !_targetLocker.CanBeOpened(UserDataHandler.Singleton.UserData.Id))
                {
                    _targetLocker.Open();
                    return;
                }

                AuthorizeServerRpc(UserDataHandler.Singleton.UserData.Id);
                return;
            }

            base.Interact();
        }

        public void Lock(Locker locker)
            => _targetLocker = locker;
        
        public bool IsLocked()
            => _targetLocker != null;
    }
}