using System.Linq;
using Animation_System;
using Player_Controller;
using PlayerDeathSystem;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.NetWorking
{
    public class PlayerStaffSpawner : NetworkBehaviour
    {
        public static PlayerStaffSpawner Singleton { get; private set; }

        [SerializeField] private PlayerNetCode _playerNetCodePref;

        private void Awake()
            => Singleton = this;

  
        [ServerRpc(RequireOwnership = false)]
        public void SpawnPlayerServerRpc(Vector3 pos, Quaternion rot, ulong ownerClientId, int instanceId = -1)
        {
            if(!IsServer) return;
            var obj = Instantiate(_playerNetCodePref, pos, rot);
            var networkObj = obj.GetComponent<NetworkObject>();
            networkObj.DontDestroyWithOwner = true;
            networkObj.Spawn(); 
            networkObj.ChangeOwnership(ownerClientId);
            if (instanceId != -1)
                TryAssignBackPackToPlayer((ulong)instanceId, obj);
        }

        private void TryAssignBackPackToPlayer(ulong instanceId, PlayerNetCode playerNetCode)
        {
            var backPacks = FindObjectsOfType<BackPack>().ToList();
            foreach (var backPack in backPacks)
            {
                if(backPack.NetworkObject.NetworkObjectId != instanceId) continue;
                playerNetCode.CharacterInventory.AssignCells(backPack.ItemsNetData.Value);
                backPack.DespawnServerRpc();
                return;
            }
        }
    }
}