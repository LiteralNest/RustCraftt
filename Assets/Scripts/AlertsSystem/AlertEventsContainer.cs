using System;

namespace AlertsSystem
{
    public static class AlertEventsContainer
    {
        public static Action<string, int> OnInventoryItemAdded { get; set; }
        public static Action<string, int> OnInventoryItemRemoved { get; set; }
        public static Action<string, int, int> OnCreatingQueueAlertDataChanged { get; set; }

        public static Action<bool> OnComfortAlert { get; set; }
        public static Action<bool> OnBleedAlert { get; set; }
        public static Action<bool> OnStarvingAlert { get; set; }
        public static Action<bool> OnDehydratedAlert { get; set; }
        public static Action<int, bool> OnWorkBenchAlert { get; set; }
        public static Action<bool> OnBuildingBlockedAlert { get; set; }
        public static Action<bool> OnBuildingUnblockedAlert { get; set; }
    }
}