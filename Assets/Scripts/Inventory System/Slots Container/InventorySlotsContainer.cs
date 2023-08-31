public class InventorySlotsContainer : SlotsContainer
{

    public override void AddItemToDesiredSlot(Item item, int count)
    {
        GlobalEventsContainer.InventoryItemAdded?.Invoke(new InventoryCell(item, count));
        base.AddItemToDesiredSlot(item, count);
        GlobalEventsContainer.InventoryDataChanged?.Invoke();
    }

    public override void DeleteSlot(Item item, int count)
    {
        base.DeleteSlot(item, count);
        GlobalEventsContainer.InventoryDataChanged?.Invoke();
    }

    public override void RemoveItemCountAt(Item item, int count, int index)
    {
        base.RemoveItemCountAt(item, count, index);
        GlobalEventsContainer.InventoryItemRemoved?.Invoke(new InventoryCell(item, count));
    }
}