using System;
using System.Collections.Generic;
using SurroundingEffectsSystem;

public static class GlobalEventsContainer
{
    public static Action<InventoryCell> InventoryItemAdded { get; set; }
    public static Action<InventoryCell> InventoryItemRemoved { get; set; }
    public static Action<List<InventoryCell>> InventoryDataShouldBeSaved { get; set; }
    public static Action InventoryDataChanged { get; set; }
    public static Action<int> ShouldActivateSlot { get; set; }
    public static Action<int, ulong> ShouldDisplayHandItem { get; set; }
    public static Action<bool> BluePrintActiveSelfSet { get; set; }
    public static Action<bool> ShouldDisplayBuildingChoosePanel { get; set; }
    public static Action<bool> ShouldDisplayBuildingStaff { get; set; }
    public static Action<bool> BuildingHammerActivated { get; set; }
    public static Action<bool> GatherButtonActivated { get; set; }
    public static Action<bool> AttackButtonActivated { get; set; }
    public static Action<bool> PickUpButtonActivated { get; set; }
    public static Action<WeaponObject> WeaponObjectAssign { get; set; }
    public static Action<bool> ShouldDisplayReloadingButton { get; set; }
    public static Action<bool> ShouldDisplayPlacingPanel { get; set; }
    public static Action<ResourceGatheringObject> ResourceGatheringObjectAssign { get; set; }
    
    public static Action<EnvironmentEffectsStateType> TemperatureChanged { get; set; }
    public static Action CriticalTemperatureReached { get; set; }
    public static Action RadiationStarted { get; set; }
    public static Action RadiationEnded { get; set; }
    public static Action PlayerDied { get; set; } 
    public static Action PlayerSpawned { get; set; }
    public static Action OnNetworkPlayerSpawned { get; set; }
}