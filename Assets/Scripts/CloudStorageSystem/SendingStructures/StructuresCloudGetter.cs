using Building_System.NetWorking;
using CloudStorageSystem.CloudStorageServices;
using CloudStorageSystem.SendingStructures.Data;
using Unity.Netcode;
using UnityEngine;

namespace CloudStorageSystem.SendingStructures
{
    public class StructuresCloudGetter : NetworkBehaviour
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
            var data = await dataHandler.LoadDataAsync<StructuresSendingDataList>(CloudStorageKeys.Structures);
            foreach (var block in data.Structures)
            {
                var pos = new Vector3(block.X, block.Y, block.Z);
                var rot = Quaternion.Euler(new Vector3(block.RotX, block.RotY, block.RotZ));
                var placingObject =
                    PlacingObjectsPool.singleton.GetInstantiatedObjectOnServer(block.StructureId, pos, rot);
                if (placingObject.TargetStorage != null && block.Inventory.Cells.Length > 0)
                    placingObject.TargetStorage.AssignCells(block.Inventory);
                placingObject.DamageHandler.SetHpOnServer(block.Hp);
            }
        }
    }
}