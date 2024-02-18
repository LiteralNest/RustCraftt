using System;
using Storage_System;
using UnityEngine;

namespace Cloud.CloudStorageSystem
{
    public static class CloudSaveEventsContainer
    {
        public static Action OnCloudSaveServiceInitialized { get; set; }

        #region BuidlingBlock

        public static Action<int, int, int> OnBuildingBlockSpawned { get; set; }
        public static Action<Vector3, int> OnBuildingBlockUpgraded { get; set; }
        public static Action<Vector3, int> OnBuildingBlockHpChanged { get; set; }
        public static Action<Vector3> OnBuildingBlockDestroyed { get; set; }

        #endregion

        #region Structures

        public static Action<int, Vector3, Vector3> OnStructureSpawned { get; set; }
        public static Action<Vector3, CustomSendingInventoryData> OnStructureInventoryChanged { get; set; }
        public static Action<int, Vector3> OnStructureHpChanged { get; set; }
        public static Action<Vector3> OnStructureDestroyed { get; set; }

        #endregion

        #region Corpes

        public static Action<int, Vector3, CustomSendingInventoryData, string, int, bool, float> OnBackPackSpawned { get; set; }
        public static Action<int, float> OnBackPackHpChanged { get; set; }
        public static Action<int, CustomSendingInventoryData> OnBackPackInventoryChanged { get; set; }
        public static Action<int> OnBackPackDestroyed { get; set; }

        #endregion
    }
}