using System;
using Fight_System.Weapon.ShootWeapon;

public static class GlobalEventsContainer
{
    public static Action<InventoryCell> OnInventoryItemAdded { get; set; }
    public static Action<InventoryCell> OnInventoryItemRemoved { get; set; }
    public static Action ShouldDisplayInventoryCells { get; set; }
    public static Action InventoryDataChanged { get; set; }
    public static Action<int, ulong> ShouldDisplayHandItem { get; set; }
    public static Action<BaseShootingWeapon> WeaponObjectAssign { get; set; }
    public static Action<MeleeShootingWeapon> WeaponMeleeObjectAssign { get; set; }
    public static Action<ResourceGatheringObject> ResourceGatheringObjectAssign { get; set; }
    
    public static Action OnCurrentItemDeleted { get; set; }

    #region Temperature & Radiation

    public static Action CriticalTemperatureReached { get; set; }
    public static Action RadiationStarted { get; set; }
    public static Action RadiationEnded { get; set; }

    #endregion

    #region Handle

    public static Action<bool> ShouldHandleAttacking { get; set; }
    public static Action<bool> ShouldHandleScopeSpear { get; set; }
    public static Action<bool> ShouldDisplayThrowButton { get; set; }

    #endregion

    #region Building Hammer

    public static Action<bool> BuildingHammerActivated { get; set; }

    #endregion
}