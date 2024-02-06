using System.Collections.Generic;
using AlertsSystem;
using Events;
using Inventory_System;
using Inventory_System.Inventory_Slot_Displayers;
using UnityEngine;

namespace Storage_System
{
    public class CharacterInventory : Storage
    {
        [SerializeField] private List<InventoryCell> _defaultItems = new List<InventoryCell>();

        private QuickSlotDisplayer _activeQuickSlot;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            SlotsDisplayer.DisplayCells();

            ItemsNetData.OnValueChanged += (oldValue, newValue) =>
            {
                if (!IsOwner) return;
                GlobalEventsContainer.InventoryDataChanged?.Invoke();
            };
        }

        private void Start()
        {
            if (InventoryClear())
            {
                foreach(var slot in _defaultItems)
                    AddItemToDesiredSlotServerRpc(slot.Item.Id, slot.Count, slot.Ammo);
            }
        }

        public override void Open(InventoryHandler handler)
        {
            SlotsDisplayer.DisplayCells();
        }

        protected override void Appear()
        {
        }

        protected override void DoAfterRemovingItem(InventoryCell cell)
        {
            GlobalEventsContainer.InventoryDataChanged?.Invoke();
        }

        protected override void DoAfterAddingItem(InventoryCell cell)
        {
            GlobalEventsContainer.InventoryDataChanged?.Invoke();
        }

        public override void AddItemToSlotWithAlert(int itemId, int count, int ammo, int hp = 100, Vector2Int range = default)
        {
            base.AddItemToSlotWithAlert(itemId, count, ammo, hp, range);
            var item = ItemFinder.singleton.GetItemById(itemId);
            AlertEventsContainer.OnInventoryItemAdded?.Invoke(item.Name, count);
        }
        
        public override void RemoveItemCountWithAlert(int slotId, int itemId, int count)
        {
            base.RemoveItemCountWithAlert(slotId, itemId, count);
            var item = ItemFinder.singleton.GetItemById(itemId);
            AlertEventsContainer.OnInventoryItemRemoved?.Invoke(item.Name, count);
        }
        
        public void SetActiveQuickSlot(QuickSlotDisplayer quickSlot)
        {
            if (_activeQuickSlot != null)
                _activeQuickSlot.OnSlotDisabled();
            _activeQuickSlot = quickSlot;
        }

        private bool InventoryClear()
        {
            foreach (var slot in ItemsNetData.Value.Cells)
            {
                if(slot.Id == -1) continue;
                return false;
            }
            return true;
        }
    }
}