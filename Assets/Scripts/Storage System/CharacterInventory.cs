namespace Storage_System
{
    public class CharacterInventory : Storage
    {
        public override void Open(InventoryHandler handler)
        {
            SlotsDisplayer.DisplayCells();
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
    }
}