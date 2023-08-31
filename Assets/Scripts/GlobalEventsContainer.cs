using System;

public static class GlobalEventsContainer
{
    public static Action<InventoryCell> InventoryItemAdded { get; set; }
    public static Action<InventoryCell> InventoryItemRemoved { get; set; }

    public static Action InventoryDataChanged { get; set; }
    public static Action<int> ShouldActivateSlot { get; set; }
    public static Action<Item> ShouldDisplayHandItem { get; set; }
}