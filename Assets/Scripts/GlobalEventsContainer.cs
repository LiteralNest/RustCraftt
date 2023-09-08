using System;

public static class GlobalEventsContainer
{
    public static Action<InventoryCell> InventoryItemAdded { get; set; }
    public static Action<InventoryCell> InventoryItemRemoved { get; set; }
    public static Action InventoryDataChanged { get; set; }
    public static Action<int> ShouldActivateSlot { get; set; }
    public static Action<Item> ShouldDisplayHandItem { get; set; }
    public static Action<bool> BluePrintActiveSelfSet { get; set; }
    public static Action<bool> ShouldDisplayBuildingChoosePanel { get; set; }
    public static Action<bool> ShouldDisplayBuildingStaff { get; set; }
    public static Action<bool> BuildingHammerActivated { get; set; }
    public static Action<bool> GatherButtonActivated { get; set; }
    public static Action<bool> AttackButtonActivated { get; set; }
}