using Events;
using Inventory_System.Inventory_Items_Displayer;
using Items_System.Items;
using Items_System.Items.Food;
using Player_Controller;
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
        

        public void Init()
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

        private bool ItemValidationPassed()
        {
            if(ItemDisplayer == null) return false;
            var item = ItemDisplayer.InventoryCell.Item;
            if(item is Food) return false;
            if (item is Resource) return false;
            return true;
        }

        public void Click()
        {
            GlobalEventsContainer.OnActiveSlotReset?.Invoke();
            if (GlobalValues.CanDragInventoryItems) return;
            var characterInventory = _inventoryHandler.CharacterInventory as CharacterInventory;
            characterInventory.SetActiveQuickSlot(this);
            if (ItemDisplayer == null)
            {
                PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
                return;
            }

            if (ItemDisplayer.InventoryCell == null) return;
            var cell = ItemDisplayer.InventoryCell;
            if (cell == null) return;
            var item = cell.Item;
            item.Click(ConnectedSlotDisplayer);
      
            if(ItemValidationPassed())
                _activeFon.SetActive(true);
        }

        public void HandleActive(bool value)
            => _activeFon.SetActive(value);

        public void OnSlotDisabled()
        {
            if (ItemDisplayer?.InventoryCell != null)
                ItemDisplayer?.InventoryCell.Item.OnClickDisabled();
        }
    }
}