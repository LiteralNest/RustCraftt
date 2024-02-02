using System.Collections;
using System.Collections.Generic;
using InteractSystem;
using Inventory_System;
using Items_System.Items.Abstract;
using Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System
{
    public abstract class Storage : NetworkBehaviour, IRaycastInteractable
    {
        [field: SerializeField]
        public NetworkVariable<CustomSendingInventoryData> ItemsNetData { get; private set; } = new();

        [field: SerializeField] public SlotsDisplayer SlotsDisplayer { get; set; }
        [SerializeField] protected GameObject _ui;

        [Header("Test")] [SerializeField] private InventoryCell _testAddingCell;
        [field: SerializeField] public int MainSlotsCount;

        protected bool Opened;

        protected void Awake()
            => SlotsDisplayer.InitCells();

        #region virtual

        public virtual void Open(InventoryHandler handler)
        {
            CurrentInventoriesHandler.Singleton.CurrentStorage = this;
            Opened = true;
            InventoryHandler.singleton.InventoryPanelsDisplayer.OpenInventory(true);
            Appear();
            _ui.SetActive(true);
            SlotsDisplayer.ResetCells();
            SlotsDisplayer.DisplayCells();
        }

        public void Close()
            => Opened = false;


        #region IRayCastInteractable

        public void HandleUi(bool value)
            => _ui.SetActive(value);

        public virtual string GetDisplayText()
            => "Open";

        public virtual void Interact()
            => Open(InventoryHandler.singleton);

        public bool CanInteract()
            => true;

        #endregion

        protected virtual void Appear()
            => ActiveInvetoriesHandler.singleton.AddActiveInventory(this);

        protected virtual void DoAfterRemovingItem(InventoryCell cell)
        {
        }

        protected virtual void DoAfterAddingItem(InventoryCell cell)
        {
        }

        #endregion

        public List<int> GetArmorSlots()
        {
            var result = new List<int>();
            for (int i = 30; i <= 33; i++)
            {
                var cell = ItemsNetData.Value.Cells[i];
                if (cell.Id == -1) continue;
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
        public void ResetItemServerRpc(int id, int interactingPlayerId)
        {
            if (!IsServer) return;
            InventoryHelper.ResetCell(id, ItemsNetData);
        }


        public void AssignCells(CustomSendingInventoryData inputList)
        {
            ItemsNetData.Value = inputList;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetItemServerRpc(int cellId, CustomSendingInventoryDataCell dataCell)
        {
            if (IsServer)
                SetItem(cellId, dataCell);
        }

        public virtual void SetItem(int cellId, CustomSendingInventoryDataCell dataCell)
            => InventoryHelper.SetItem(cellId, dataCell, ItemsNetData);

        public void RemoveItem(int itemId, int count)
        {
            RemoveItemCountServerRpc(itemId, count);
            DoAfterRemovingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        public virtual void RemoveItemCountWithAlert(int slotId, int itemId, int count)
        {
            RemoveItemCountFromSlotServerRpc(slotId, itemId, count);
        }

        protected void RemoveItemCountFromSlot(int slotId, int itemId, int count)
        {
            InventoryHelper.MinusCellCount(slotId, count, ItemsNetData);
            DoAfterRemovingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        [ServerRpc(RequireOwnership = false)]
        public void RemoveItemCountFromSlotServerRpc(int slotId, int itemId, int count)
        {
            if (!IsServer) return;
            RemoveItemCountFromSlot(slotId, itemId, count);
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


        public void AddItemToDesiredSlot(int itemId, int count, int ammo, int hp = 100, Vector2Int range = default)
        {
            if (range == default)
            {
                if (!InventoryHelper.AddItemToDesiredSlot(itemId, count, ammo, ItemsNetData,
                        new Vector2Int(0, MainSlotsCount), hp))
                {
                    InstantiatingItemsPool.sigleton.SpawnObjectOnServer(
                        new CustomSendingInventoryDataCell(itemId, count, hp, ammo),
                        transform.position + transform.forward * 1.5f);
                }
            }

            else
            {
                if (!InventoryHelper.AddItemToDesiredSlot(itemId, count, ammo, ItemsNetData, range, hp))
                {
                    InstantiatingItemsPool.sigleton.SpawnObjectOnServer(
                        new CustomSendingInventoryDataCell(itemId, count, hp, ammo),
                        transform.position + transform.forward * 1.5f);
                }
            }
        }

        public virtual void AddItemToSlotWithAlert(int itemId, int count, int ammo, int hp = 100,
            Vector2Int range = default)
        {
            AddItemToDesiredSlotServerRpc(itemId, count, ammo, hp, range);
        }

        [ServerRpc(RequireOwnership = false)]
        public void AddItemToDesiredSlotServerRpc(int itemId, int count, int ammo, int hp = 100,
            Vector2Int range = default)
        {
            if (IsServer)
                AddItemToDesiredSlot(itemId, count, ammo, hp, range);

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

        [ContextMenu("Test")]
        private void AddTestCell()
        {
            AddItemToDesiredSlotServerRpc(_testAddingCell.Item.Id, _testAddingCell.Count, 0);
        }
    }
}