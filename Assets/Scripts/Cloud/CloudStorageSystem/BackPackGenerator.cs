using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace Cloud.CloudStorageSystem
{
    public class BackPackGenerator : NetworkBehaviour
    {
        public static BackPackGenerator Singleton { get; set; }
        public int BackPackId { get; set; }

        [SerializeField] private NetworkObject _corpesPref;

        private void Awake()
            => Singleton = this;

        public void GenerateBackPack(bool wasDisconnected, int ownerId, string nickName,
            Vector3 position, Vector3 rotation, CustomSendingInventoryData data)
        {
            var backPack = Instantiate(_corpesPref.gameObject, position, Quaternion.Euler(rotation));
            backPack.GetComponent<NetworkObject>().Spawn();
            var script = backPack.GetComponent<PlayerDeathSystem.BackPack>();
            CloudSaveEventsContainer.OnBackPackSpawned?.Invoke(BackPackId, position, data, nickName, ownerId,
                wasDisconnected, script.Ore.CurrentHp.Value);
            script.NickName.Value = nickName;
            script.AssignCells(data);
            script.PlayerCorpDisplay.Init();
            script.SetWasDisconnectedAndOwnerId(wasDisconnected, ownerId);
            script.BackPackId = BackPackId;
            BackPackId--;
        }
    }
}