using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventory_System;
using Items_System.Items.Abstract;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System
{
    public abstract class Storage : NetworkBehaviour
    {
        [field: SerializeField]
        public NetworkVariable<CustomSendingInventoryData> ItemsNetData { get; private set; } = new();

        [field: SerializeField] public SlotsDisplayer SlotsDisplayer { get; set; }
        [SerializeField] protected GameObject _ui;

        [Header("Test")] [SerializeField] private InventoryCell _testAddingCell;
        [field: SerializeField] public int MainSlotsCount;

        protected void Awake()
        {
            gameObject.tag = "LootBox";
            SlotsDisplayer.InitCells();
        }

        #region virtual

        public virtual void Open(InventoryHandler handler)
        {
            CurrentInventoriesHandler.Singleton.CurrentStorage = this;
            InventoryHandler.singleton.InventoryPanelsDisplayer.OpenInventory(true);
            Appear();
            _ui.SetActive(true);
            SlotsDisplayer.DisplayCells();
        }

        public void HandleUi(bool value)
            => _ui.SetActive(value);

        protected virtual void Appear()
            => ActiveInvetoriesHandler.singleton.AddActiveInventory(this);

        protected virtual void DoAfterRemovingItem(InventoryCell cell)
        {
        }

        protected virtual void DoAfterAddingItem(InventoryCell cell)
        {
        }

        protected virtual void DoAfterResetItem()
        {
        }

        #endregion

        public List<int> GetArmorSlots()
        {
            var result = new List<int>();
            for(int i = 30; i <= 33; i++)
            {
                var cell = ItemsNetData.Value.Cells[i];
                if(cell.Id == -1) continue;
                result.Add(cell.Id);
            }
            return result;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetItemsServerRpc(CustomSendingInventoryData data)
        {
            if (!IsServer) return;
            ItemsNetData.Value = data;
        }


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            ItemsNetData.OnValueChanged += (oldValue, newValue) =>
            {
                if (SlotsDisplayer != null)
                    SlotsDisplayer.DisplayCells();
            };
        }

        [ServerRpc(RequireOwnership = false)]
        public void ResetItemServerRpc(int id)
        {
            if (IsServer)
                InventoryHelper.ResetCell(id, ItemsNetData);
            DoAfterResetItem();
        }

        public void AssignCells(CustomSendingInventoryData inputList)
        {
            ItemsNetData.Value = inputList;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetItemAndResetCellServerRpc(int addingCellId, CustomSendingInventoryDataCell dataCell,
            int resetingCellId)
        {
            if (!IsServer) return;
            InventoryHelper.SetItemAndResetCell(addingCellId, dataCell, resetingCellId, ItemsNetData);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetItemServerRpc(int cellId, CustomSendingInventoryDataCell dataCell)
        {
            if (IsServer)
                SetItem(cellId, dataCell);
        }

        protected virtual void SetItem(int cellId, CustomSendingInventoryDataCell dataCell)
            => InventoryHelper.SetItem(cellId, dataCell, ItemsNetData);

        public void RemoveItem(int itemId, int count)
        {
            RemoveItemCountServerRpc(itemId, count);
            DoAfterRemovingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        [ServerRpc(RequireOwnership = false)]
        public void RemoveItemCountFromSlotServerRpc(int slotId, int itemId, int count)
        {
            if (!IsServer) return;
            InventoryHelper.MinusCellCount(slotId, count, ItemsNetData);
            DoAfterRemovingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        [ServerRpc(RequireOwnership = false)]
        protected void RemoveItemCountServerRpc(int itemId, int count)
        {
            for (int i = 0; i < ItemsNetData.Value.Cells.Length; i++)
            {
                if (ItemsNetData.Value.Cells[i].Id == itemId)
                {
                    InventoryHelper.MinusCellCount(i, count, ItemsNetData);
                    return;
                }
            }

            DoAfterRemovingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        public void RemoveItems(List<InventoryCell> cells)
        {
            foreach (var cell in cells)
                RemoveItemCountServerRpc(cell.Item.Id, cell.Count);
        }


        [ServerRpc(RequireOwnership = false)]
        public void AddItemToDesiredSlotServerRpc(int itemId, int count, int ammo, int hp = 100, Vector2Int range = default)
        {
            if (IsServer)
            {
                if (range == default)
                {
                    if (!InventoryHelper.AddItemToDesiredSlot(itemId, count, ammo, ItemsNetData,
                            new Vector2Int(0, MainSlotsCount), hp))
                    {
                        InstantiatingItemsPool.sigleton.SpawnObjectServerRpc(new CustomSendingInventoryDataCell(itemId, count, hp, ammo),
                            transform.forward * 1.5f);
                    }
                }

                else
                {
                    if (!InventoryHelper.AddItemToDesiredSlot(itemId, count, ammo, ItemsNetData, range, hp))
                    {
                        InstantiatingItemsPool.sigleton.SpawnObjectServerRpc(new CustomSendingInventoryDataCell(itemId, count, hp, ammo),
                            transform.forward * 1.5f);
                    }
                }
            }

            DoAfterAddingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        private IEnumerator AddItemToServerWithRoutine(int itemId, int count, int ammo)
        {
            yield return new WaitForSeconds(0.2f);
            AddItemToDesiredSlotServerRpc(itemId, count, ammo);
        }

        public void AddCraftedItem(int itemId, int count, int ammo)
        {
            StartCoroutine(AddItemToServerWithRoutine(itemId, count, ammo));
        }

        public bool EnoughMaterials(List<InventoryCell> inputCells)
            => InventoryHelper.EnoughMaterials(inputCells, ItemsNetData);

        public int GetItemCount(int id)
            => InventoryHelper.GetItemCount(id, ItemsNetData);

        public virtual bool CanAddItem(Item item, int cellId)
            => true;

        public virtual int GetAvailableCellIndexForMovingItem(Item item)
            => InventoryHelper.GetFreeCellId(ItemsNetData, new Vector2Int(0, MainSlotsCount));

        protected void RemoveItem(Item item, int count)
            => InventoryHelper.RemoveItemCount(item.Id, count, ItemsNetData);

        [ContextMenu("Test")]
        private void AddTestCell()
        {
            AddItemToDesiredSlotServerRpc(_testAddingCell.Item.Id, _testAddingCell.Count, 0);
        }
    }
}