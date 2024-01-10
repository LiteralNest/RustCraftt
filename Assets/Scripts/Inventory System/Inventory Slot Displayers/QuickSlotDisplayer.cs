using Storage_System;
using UnityEngine;

namespace Inventory_System.Inventory_Slot_Displayers
{
    public class QuickSlotDisplayer : MonoBehaviour
    {
        [SerializeField] private InventoryHandler _inventoryHandler;

        [Header("UI")] [SerializeField] private GameObject _activeFon;

        public SlotDisplayer ConnectedSlotDisplayer { get; set; }
        public ItemDisplayer ItemDisplayer { get; private set; }

        public void ClearSlot()
        {
            if (!ItemDisplayer) return;
            Destroy(ItemDisplayer.gameObject);
            ItemDisplayer = null;
            ConnectedSlotDisplayer = null;
        }

        public void AssignItemDisplayer(ItemDisplayer itemDisplayer)
        {
            ItemDisplayer = itemDisplayer;
            Destroy(itemDisplayer.GetComponent<DoubleTapHandler>());
        }

        public void Click()
        {
            if (GlobalValues.CanDragInventoryItems) return;
            var characterInventory = _inventoryHandler.CharacterInventory as CharacterInventory;
            characterInventory.SetActiveQuickSlot(this);
            if (ItemDisplayer == null)
            {
                _inventoryHandler.InHandObjectsContainer.SetDefaultHands();
                return;
            }

            if (ItemDisplayer.InventoryCell == null) return;
            var cell = ItemDisplayer.InventoryCell;
            if (cell == null) return;
            var item = cell.Item;
            item.Click(ConnectedSlotDisplayer);
        }

        public void OnSlotDisabled()
        {
            if (ItemDisplayer?.InventoryCell != null) 
                ItemDisplayer?.InventoryCell.Item.OnClickDisabled();
        }
    }
}