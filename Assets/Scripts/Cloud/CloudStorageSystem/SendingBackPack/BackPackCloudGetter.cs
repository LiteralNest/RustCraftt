using Cloud.CloudStorageSystem.CloudStorageServices;
using Cloud.CloudStorageSystem.SendingBackPack.Data;
using Unity.Netcode;
using UnityEngine;

namespace Cloud.CloudStorageSystem.SendingBackPack
{
    public class BackPackCloudGetter : NetworkBehaviour
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
            var data = await dataHandler.LoadDataAsync<BackPackListData>(CloudStorageKeys.BackPacks);
            var backPacks = data.Slots;
            foreach (var backPack in backPacks)
            {
                var position = new Vector3(backPack.X, backPack.Y, backPack.Z);
                BackPackGenerator.Singleton.GenerateBackPack(backPack.WasDisconnected, backPack.OwnerId,
                    backPack.NickName, position, Vector3.zero, backPack.ItemsNetData);
            }
        }
    }
}