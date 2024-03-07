using System;
using Player_Controller;
using RespawnSystem.SleepingBag;

namespace Events
{
    public static class GlobalEventsContainer
    {
        public static Action InventoryClosed { get; set; }
        public static Action InventoryDataChanged { get; set; }
        public static Action<int, ulong> ShouldDisplayHandItem { get; set; }
        public static Action BluePrintDeactivated { get; set; }
        public static Action OnCurrentItemDeleted { get; set; }
        public static Action<bool> OnMicrophoneButtonClicked { get; set; }
        public static Action<SleepingBag> SleepingBagSpawned { get; set; }
        public static Action<bool> OnMainHudHandle { get; set; }
        public static Action OnPlayerStandUp { get; set; }
        public static Action OnPlayerKnockDown { get; set; }
        public static Action OnMapOpened { get; set; }
        public static Action OnActiveSlotReset { get; set; }
        public static Action<string> OnChatMessageCreated { get; set; }

        #region Inventory

        public static Action InventoryItemDragged { get; set; }

        #endregion

        #region Temperature & Radiation

        public static Action CriticalTemperatureReached { get; set; }
        public static Action RadiationStarted { get; set; }
        public static Action RadiationEnded { get; set; }

        #endregion

        #region Handle

        public static Action<bool> ShouldHandleAttacking { get; set; }

        #endregion
        

        #region Player Net Code

        public static Action<PlayerNetCode> PlayerNetCodeAssigned { get; set; }
        public static Action OnZeroHp { get; set; }

        #endregion
    }
}