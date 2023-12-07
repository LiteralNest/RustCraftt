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
            if (!IsOwner) return;
            if (_userId.Value != userId) return;
            var point = PlayerSpawnManager.Singleton.GetRandomSpawnPoint();
            if (spawnPoint == new Vector3(0, -1000000, 0))
                point = spawnPoint;
            PlayerStaffSpawner.Singleton.SpawnPlayerServerRpc(point, Quaternion.identity, _userId.Value,
                GetComponent<NetworkObject>().OwnerClientId);
        }
        
        public void Respawn(Vector3 spawnPoint)
        {
            List<PlayerStartSpawner> players = FindObjectsOfType<PlayerStartSpawner>().ToList();
            foreach (var player in players)
            {
                if(!player.IsOwner) continue;
                player.Respawn(UserDataHandler.singleton.UserData.Id, spawnPoint);
            }
          
        }
        

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsServer) return;
            List<PlayerKiller> players = FindObjectsOfType<PlayerKiller>().ToList();
            foreach (var player in players)
            {
                if (player.UserId != _userId.Value) continue;
                player.DieServerRpc();
            }
        }
    }
}