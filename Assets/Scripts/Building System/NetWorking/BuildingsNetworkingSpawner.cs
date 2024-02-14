using Building_System.Building.Blocks;
using CloudStorageSystem;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.NetWorking
{
    [RequireComponent(typeof(BuildingObjectsPool))]
    public class BuildingsNetworkingSpawner : NetworkBehaviour
    {
        public static BuildingsNetworkingSpawner Singleton { get; set; }

        [SerializeField] private BuildingObjectsPool _buildingObjectsPool;

        private void Awake()
        {
            Singleton = this;
            _buildingObjectsPool = GetComponent<BuildingObjectsPool>();
        }


        [ServerRpc(RequireOwnership = false)]
        public void SpawnPrefServerRpc(int id, Vector3 pos, Quaternion rot, bool shouldPlaySound)
        {
            if (!IsServer) return;
            GetSpawnedPrefOnServer(id, pos, rot, shouldPlaySound);
        }

        public BuildingBlock GetSpawnedPrefOnServer(int id, Vector3 pos, Quaternion rot, bool shouldPlaySound)
        {
            var obj = Instantiate(_buildingObjectsPool.GetObjectByPoolId(id), pos, rot);
            CloudSaveEventsContainer.OnBuildingBlockSpawned?.Invoke((int)pos.x, (int)pos.y, (int)pos.z);
            obj.DontDestroyWithOwner = true;
            obj.Spawn();
            if (shouldPlaySound)
                obj.GetComponent<BuildingBlock>().PlaySound();
            return obj.GetComponent<BuildingBlock>();
        }
    }
}