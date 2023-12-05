using System.Collections.Generic;
using System.Linq;
using Building_System.NetWorking;
using PlayerDeathSystem;
using Unity.Netcode;
using UnityEngine;
using Web.User;

namespace Multiplayer.PlayerSpawning
{
    public class PlayerStartSpawner : NetworkBehaviour
    {
        private NetworkVariable<int> _userId = new(-1, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private void Start()
        {
            if (!IsOwner) return;
            _userId.Value = UserDataHandler.singleton.UserData.Id;
            PlayerStaffSpawner.Singleton.SpawnPlayerServerRpc(PlayerSpawnManager.Singleton.GetRandomSpawnPoint(),
                Quaternion.identity, _userId.Value, GetComponent<NetworkObject>().OwnerClientId);
        }

        public void Respawn(int userId, Vector3 spawnPoint)
        {
            var point = PlayerSpawnManager.Singleton.GetRandomSpawnPoint();
            if (spawnPoint == new Vector3(0, -1000000, 0))
                point = spawnPoint;
            if (!IsOwner) return;
            if (_userId.Value != userId) return;
            PlayerStaffSpawner.Singleton.SpawnPlayerServerRpc(point, Quaternion.identity, _userId.Value,
                GetComponent<NetworkObject>().OwnerClientId);
        }

        [ServerRpc(RequireOwnership = false)] 
        public void RespawnServerRpc(Vector3 spawnPoint)
        {
            if (!IsServer) return;
            TryRespawnClientRpc(spawnPoint);
        }

        [ClientRpc]
        private void TryRespawnClientRpc(Vector3 spawnPoint)
        {
            if (!IsOwner) return;
            List<PlayerStartSpawner> players = FindObjectsOfType<PlayerStartSpawner>().ToList();
            foreach (var player in players)
                player.Respawn(UserDataHandler.singleton.UserData.Id, spawnPoint);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsServer) return;
            List<PlayerKiller> players = FindObjectsOfType<PlayerKiller>().ToList();
            foreach (var player in players)
            {
                if (player.UserId != _userId.Value) continue;
                player.RemoveMainComponentsServerRpc();
            }
        }
    }
}