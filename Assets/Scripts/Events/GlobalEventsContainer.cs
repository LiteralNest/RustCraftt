using System;
using Fight_System.Weapon.ShootWeapon;
using Player_Controller;
using RespawnSystem.SleepingBag;

namespace Events
{
    public static class GlobalEventsContainer
    {
        public static Action InventoryClosed { get; set; }

        public static Action<InventoryCell> OnInventoryItemAdded { get; set; }
        public static Action<InventoryCell> OnInventoryItemRemoved { get; set; }
        public static Action InventoryDataChanged { get; set; }
        public static Action<int, ulong> ShouldDisplayHandItem { get; set; }
        public static Action<BaseShootingWeapon> WeaponObjectAssign { get; set; }
        public static Action<MeleeShootingWeapon> WeaponMeleeObjectAssign { get; set; }
        public static Action<ResourceGatheringObject> ResourceGatheringObjectAssign { get; set; }
        public static Action BluePrintDeactivated { get; set; }
        public static Action OnCurrentItemDeleted { get; set; }
        public static Action<bool> OnMicrophoneButtonClicked { get; set; }
        public static Action<SleepingBag> SleepingBagSpawned { get; set; }
        
        public static Action<bool> OnMainHudHandle { get; set; }
        public static Action OnMapOpened { get; set; }

        #region Temperature & Radiation

        public static Action CriticalTemperatureReached { get; set; }
        public static Action RadiationStarted { get; set; }
        public static Action RadiationEnded { get; set; }

        #endregion

        #region Handle

        public static Action<bool> ShouldHandleAttacking { get; set; }

        #endregion

        #region Building Hammer

        public static Action<bool> BuildingHammerActivated { get; set; }

        #endregion

        #region Player Net Code

        public static Action<PlayerNetCode> PlayerNetCodeAssigned { get; set; }

        #endregion
    }
}