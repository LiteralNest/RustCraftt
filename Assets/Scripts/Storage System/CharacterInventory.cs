using System.Threading.Tasks;

namespace Storage_System
{
    public class CharacterInventory : Storage
    {
        private async void Start()
        {
            await Task.Delay(1100);
            var handler = InventoryHandler.singleton;
            handler.InventoryPanelsDisplayer.HandleInventory(true);
            Open(handler);
            handler.InventoryPanelsDisplayer.HandleInventory(false);
        }
        
        protected override void Appear()
        {
        }

        protected override void DoAfterRemovingItem(InventoryCell cell)
        {
            GlobalEventsContainer.OnInventoryItemRemoved?.Invoke(cell);
            GlobalEventsContainer.InventoryDataChanged?.Invoke();
        }

        protected override void DoAfterAddingItem(InventoryCell cell)
        {
            GlobalEventsContainer.OnInventoryItemAdded?.Invoke(cell);
            GlobalEventsContainer.InventoryDataChanged?.Invoke();
        }

        public override void Open(InventoryHandler handler)
        {
            SlotsDisplayer = handler.InventorySlotsDisplayer;
            base.Open(handler);
        }
    }
}