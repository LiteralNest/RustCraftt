using System.Collections.Generic;
using Events;
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

            if (InventoryClear())
            {
                foreach(var slot in _defaultItems)
                    AddItemToDesiredSlotServerRpc(slot.Item.Id, slot.Count, slot.Ammo);
            }
            
            ItemsNetData.OnValueChanged += (oldValue, newValue) =>
            {
                if (!IsOwner) return;
                GlobalEventsContainer.InventoryDataChanged?.Invoke();
            };
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