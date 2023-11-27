using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Storage_System
{
    public abstract class Storage : NetworkBehaviour
    {
       [field:SerializeField] public NetworkVariable<CustomSendingInventoryData> ItemsNetData { get; set; } = new();
       [field:SerializeField] public SlotsDisplayer SlotsDisplayer { get; set; }

        [Header("Test")] [SerializeField] private InventoryCell _testAddingCell;


        protected void Awake()
        {
            if(SlotsDisplayer == null)
                SlotsDisplayer = GetComponent<SlotsDisplayer>();
            gameObject.tag = "LootBox";
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                if (ItemsNetData.Value.Cells.Length == 0)
                {
                    ItemsNetData.Value =
                        new CustomSendingInventoryData(
                            new CustomSendingInventoryDataCell[SlotsDisplayer.CellDisplayers.Count]);
                    ResetItemsServerRpc();
                }
            }
            base.OnNetworkSpawn();
        }

        #region virtual

        public virtual void CheckCells()
        {
        }

        public virtual void Open(InventoryHandler handler)
        {
            Appear();
            SlotsDisplayer.AssignStorage(this);
            SlotsDisplayer.InitCells();
            SlotsDisplayer.DisplayCells();
        }

        protected virtual void Appear()
            => ActiveInvetoriesHandler.singleton.AddActiveInventory(this);

        protected virtual void DoAfterRemovingItem(InventoryCell cell)
        {
        }

        protected virtual void DoAfterAddingItem(InventoryCell cell)
        {
        }

        #endregion

        [ServerRpc(RequireOwnership = false)]
        public void ResetItemsServerRpc()
        {
            if (IsServer)
                InventoryHelper.ResetItems(ItemsNetData);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void ResetItemServerRpc(int id)
        {
            if (IsServer)
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
                InventoryHelper.SetItem(cellId, dataCell, ItemsNetData);
        }

        public void AddItem(int cellId, int itemId, int count)
        {
            Item item = ItemFinder.singleton.GetItemById(itemId);
            InventoryHelper.SetItem(cellId, new CustomSendingInventoryDataCell(item.Id, count, -1), ItemsNetData);
            DoAfterAddingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddItemCountServerRpc(CustomSendingInventoryDataCell cell)
        {
            if (IsServer)
                InventoryHelper.AddCell(cell, ItemsNetData);
        }

        public void RemoveItem(int itemId, int count)
        {
            RemoveItemCountServerRpc(itemId, count);
            DoAfterRemovingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        [ServerRpc(RequireOwnership = false)]
        protected void RemoveItemCountServerRpc(int itemId, int count)
        {
            for (int i = 0; i < ItemsNetData.Value.Cells.Length; i++)
            {
                InventoryHelper.MinusCellCount(i, count, ItemsNetData);
            }

            DoAfterRemovingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        public void RemoveItems(List<InventoryCell> cells)
        {
            foreach (var cell in cells)
                RemoveItemCountServerRpc(cell.Item.Id, cell.Count);
        }

        private IEnumerator AddItemToServerWithRoutine(int itemId, int count)
        {
            yield return new WaitForSeconds(0.2f);
            AddItemToDesiredSlotServerRpc((ushort)itemId, (ushort)count);
        }

        public void AddCraftedItem(int itemId, int count)
        {
            StartCoroutine(AddItemToServerWithRoutine(itemId, count));
            DoAfterAddingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        [ServerRpc(RequireOwnership = false)]
        public void AddItemToDesiredSlotServerRpc(int itemId, int count)
        {
            if (IsServer)
                InventoryHelper.AddItemToDesiredSlot(itemId, count, ItemsNetData);
        }


        public bool EnoughMaterials(List<InventoryCell> inputCells)
            => InventoryHelper.EnoughMaterials(inputCells, ItemsNetData);

        public int GetItemCount(int id)
            => InventoryHelper.GetItemCount(id, ItemsNetData);
        

        public virtual bool CanAddItem(Item item)
            => true;

        protected void RemoveItem(Item item, int count)
        {
            InventoryHelper.RemoveItemCount(item.Id, count, ItemsNetData);
        }
        
        [ContextMenu("Test")]
        private void AddTestCell()
            => AddItemToDesiredSlotServerRpc(_testAddingCell.Item.Id, _testAddingCell.Count);
    }
}