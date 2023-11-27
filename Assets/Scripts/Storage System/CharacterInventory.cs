namespace Storage_System
{
    public class CharacterInventory : Storage
    {
        protected override void Appear()
        {
        }

        protected override void DoAfterRemovingItem(InventoryCell cell)
            => GlobalEventsContainer.OnInventoryItemRemoved?.Invoke(cell);

        protected override void DoAfterAddingItem(InventoryCell cell)
            => GlobalEventsContainer.OnInventoryItemAdded?.Invoke(cell);

        public override void Open(InventoryHandler handler)
        {
            SlotsDisplayer = handler.InventorySlotsDisplayer;
            base.Open(handler);
        }
    }
}