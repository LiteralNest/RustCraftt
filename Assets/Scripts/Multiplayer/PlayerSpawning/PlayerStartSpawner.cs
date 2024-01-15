using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animation_System;
using Building_System.NetWorking;
using PlayerDeathSystem;
using Storage_System;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace Multiplayer.PlayerSpawning
{
    public class PlayerStartSpawner : NetworkBehaviour
    {
        private NetworkVariable<int> _userId = new(-1, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [field: SerializeField] public AnimationsManager AnimationsManager { get; private set; }

        private async void Start()
        {
            if (!IsOwner) return;
            _userId.Value = UserDataHandler.Singleton.UserData.Id;
            TryConnectServerToBackPack();
            await Task.Delay(1500);
            if (!IsOwner) return;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsServer) return;
            List<PlayerKiller> players = FindObjectsOfType<PlayerKiller>().ToList();
            foreach (var player in players)
            {
                if (player.UserId != _userId.Value) continue;
                player.DisconnectServerRpc(_userId.Value);
            }
        }
        
        public void Respawn(Vector3 spawnPoint)
        {
            List<PlayerStartSpawner> players = FindObjectsOfType<PlayerStartSpawner>().ToList();
            foreach (var player in players)
            {
                if (!player.IsOwner) continue;
                player.Respawn(UserDataHandler.Singleton.UserData.Id, spawnPoint);
                return;
            }
        }

        private void TryConnectServerToBackPack()
        {
            var backPacks = FindObjectsOfType<BackPack>().ToList();
            foreach (var backPack in backPacks)
            {
                if (backPack == null) continue;
                if (!(backPack.WasDisconnected.Value && backPack.OwnerId.Value == _userId.Value)) return;
                PlayerStaffSpawner.Singleton.SpawnPlayerServerRpc(backPack.transform.position + new Vector3(0, 1, 0),
                    backPack.transform.rotation, GetComponent<NetworkObject>().OwnerClientId, (int)backPack.NetworkObject.NetworkObjectId);
                return;
            }

            SpawnPlayerInRandomPoint();
        }

        private void SpawnPlayerInRandomPoint()
        {
            PlayerStaffSpawner.Singleton.SpawnPlayerServerRpc(PlayerSpawnManager.Singleton.GetRandomSpawnPoint(),
                Quaternion.identity, GetComponent<NetworkObject>().OwnerClientId);
        }

        private void Respawn(int userId, Vector3 spawnPoint)
        {
            if (_userId.Value != userId) return;
            var point = PlayerSpawnManager.Singleton.GetRandomSpawnPoint();
            if (spawnPoint == new Vector3(0, -1000000, 0))
                point = spawnPoint;
            PlayerStaffSpawner.Singleton.SpawnPlayerServerRpc(point, Quaternion.identity,
                GetComponent<NetworkObject>().OwnerClientId);
        }
    }
}