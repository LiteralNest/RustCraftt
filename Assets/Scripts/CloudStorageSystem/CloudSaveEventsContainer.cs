using System;
using UnityEngine;

namespace CloudStorageSystem
{
    public static class CloudSaveEventsContainer
    {
        public static Action OnCloudSaveServiceInitialized { get; set; }
        public static Action<int, int, int> OnBuildingBlockSpawned { get; set; }
        public static Action<Vector3, int> OnBuildingBlockUpgraded { get; set; }
        public static Action<Vector3, int> OnBuildingBlockHpChanged { get; set; }
    }
}