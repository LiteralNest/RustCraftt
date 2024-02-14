using System;
using Storage_System;
using UnityEngine;

namespace CloudStorageSystem
{
    public static class CloudSaveEventsContainer
    {
        public static Action OnCloudSaveServiceInitialized { get; set; }
        
        #region BuidlingBlock
        
        public static Action<int, int, int> OnBuildingBlockSpawned { get; set; }
        public static Action<Vector3, int> OnBuildingBlockUpgraded { get; set; }
        public static Action<Vector3, int> OnBuildingBlockHpChanged { get; set; }

        #endregion

        #region Structures

        public static Action<int, Vector3, Vector3> OnStructureSpawned { get; set; }
        public static Action<Vector3, CustomSendingInventoryData> OnStructureInventoryChanged { get; set; }
        public static Action<int, Vector3> OnStructureHpChanged { get; set; }

        #endregion
    }
}