using Inventory_System.Inventory_Items_Displayer;
using Player_Controller;
using Storage_System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory_System.Inventory_Slot_Displayers
{
    public abstract class SlotDisplayer : MonoBehaviour, IDropHandler
    {
        public ItemDisplayer ItemDisplayer { get; set; }
        [field: SerializeField] public bool IsQuickSlot { get; private set; }
        [field: SerializeField] public bool CanSetSlot { get; set; }

        public int Index { get; private set; }

        public SlotsDisplayer InventorySlotsDisplayer { get; protected set; }
        public Storage Inventory { get; protected set; }


        protected virtual void Drop(PointerEventData eventData)
        {
            var itemDisplayer = eventData.pointerDrag.GetComponent<InventoryItemDisplayer>();
            if (TrySetItem(itemDisplayer))
            {
                if (itemDisplayer != null)
                    Destroy(itemDisplayer.gameObject);
                PlayerNetCode.Singleton.ItemInfoHandler.ResetPanel();
                return;
            }
         
            itemDisplayer.SetPosition();
        }

        public void OnDrop(PointerEventData eventData)
            => Drop(eventData);

        public void Init(int index, SlotsDisplayer slotsDisplayer, Storage targetStorage)
        {
            Index = index;
            InventorySlotsDisplayer = slotsDisplayer;
            Inventory = targetStorage;
            ItemDisplayer = null;
        }

        public void DisplayItem(ItemDisplayer itemDisplayer)
        {
            if (ItemDisplayer != null) Destroy(ItemDisplayer.gameObject);
            ItemDisplayer = itemDisplayer;
            ItemDisplayer.SetNewCell(this);
        }

        private void SetItem(ItemDisplayer itemDisplayer)
        {
            if(itemDisplayer.PreviousCell)
                itemDisplayer.PreviousCell.Inventory.ResetItemServerRpc(itemDisplayer.PreviousCell.Index, (int)PlayerNetCode.Singleton.OwnerClientId);
            itemDisplayer.DoAfterMovingItemOut();
            var cell = itemDisplayer.InventoryCell;
            Inventory.SetItemServerRpc(Index,
                new CustomSendingInventoryDataCell(cell.Item.Id, cell.Count, cell.Hp, cell.Ammo));
        }

        private void ResetItem()
        {
            ItemDisplayer = null;
        }

        public virtual void ResetItemWhileDrag()
            => ItemDisplayer = null;

        private void ClearPlace(Transform place)
        {
            foreach (Transform child in place)
                Destroy(child.gameObject);
        }

        public void DestroyItem()
        {
            if (transform.childCount != 0)
                ClearPlace(transform);
            ResetItem();
        }

        private bool CheckForFree(ItemDisplayer itemDisplayer)
        {
            if (ItemDisplayer) return false;
            SetItem(itemDisplayer);
            return true;
        }

        private bool TryStack(ItemDisplayer displayer, out bool wasStacking)
        {
            var cell = displayer.InventoryCell;
            wasStacking = false;
            if (cell.Item == null || cell.Item != ItemDisplayer.InventoryCell.Item) return false;
            wasStacking = true;
            ItemDisplayer.StackCount(displayer);
            return true;
        }

        private void Swap(ItemDisplayer itemDisplayer)
        {
            InventoryHelper.SwapCells(Index, Inventory, itemDisplayer.PreviousCell.Index,
                itemDisplayer.PreviousCell.Inventory);
        }

        protected virtual bool TrySetItem(ItemDisplayer itemDisplayer)
        {
            if (!CanSetSlot) return false;
            if (!Inventory.CanAddItem(itemDisplayer.InventoryCell.Item, Index)) return false;
            if (CheckForFree(itemDisplayer)) return true;
            if (TryStack(itemDisplayer, out bool wasStacking)) return true;
            if (wasStacking) return false;
            Swap(itemDisplayer);
            return true;
        }
    }
}