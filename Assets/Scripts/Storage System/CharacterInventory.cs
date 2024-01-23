using Events;
using Inventory_System.Inventory_Slot_Displayers;

namespace Storage_System
{
    public class CharacterInventory : Storage
    {
        private QuickSlotDisplayer _activeQuickSlot;
        
        public  override  void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            SlotsDisplayer.DisplayCells();
            
            ItemsNetData.OnValueChanged += (oldValue, newValue) =>
            {
                if(!IsOwner) return;
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
            if(_activeQuickSlot != null)
                _activeQuickSlot.OnSlotDisabled();
            _activeQuickSlot = quickSlot;
        }
    }
}