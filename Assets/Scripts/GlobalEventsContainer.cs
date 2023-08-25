using System;

public static class GlobalEventsContainer
{
    public static Action<InventoryItem> InventoryItemAdded { get; set; }
}