using Building_System.NetWorking;
using CloudStorageSystem.CloudStorageServices;
using Unity.Netcode;
using UnityEngine;

namespace CloudStorageSystem.Blocks
{
    public class BuildingStructuresCloudGetter : NetworkBehaviour
    {
        private void OnDisable()
        {
            CloudSaveEventsContainer.OnCloudSaveServiceInitialized -= LoadObjectsAsync;
        }

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;
            CloudSaveEventsContainer.OnCloudSaveServiceInitialized += LoadObjectsAsync;
        }

        private async void LoadObjectsAsync()
        {
            ServerDataHandler dataHandler = new();
            var data = await dataHandler.LoadDataAsync<SendingBlocksData>("Blocks");
            var blocks = data.BlockPositions;
            foreach (var block in blocks)
            {
                var blockObject = BuildingsNetworkingSpawner.Singleton.GetSpawnedPrefOnServer(0,
                    new Vector3(block.X, block.Y, block.Z), Quaternion.identity, false);
                blockObject.SetLevel((ushort)block.Level);
                blockObject.SetHp(block.Hp);
            }
        }
    }
}