using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.NetWorking
{
    public class PlayerStaffSpawner : NetworkBehaviour
    {
        public static PlayerStaffSpawner Singleton { get; private set; }

        [SerializeField] private PlayerNetCode _playerNetCodePref;
        [SerializeField] private GameObject _sleepPref;

        private void Awake()
            => Singleton = this;
    
        [ServerRpc(RequireOwnership = false)]
        public void SpawnSleepServerRpc(Vector3 pos, Quaternion rot)
        {
            if(!IsServer) return;
            var obj = Instantiate(_sleepPref, pos, rot);
            obj.GetComponent<NetworkObject>().Spawn();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnPlayerServerRpc(Vector3 pos, Quaternion rot, int id, ulong ownerClientId)
        {
            if(!IsServer) return;
            var obj = Instantiate(_playerNetCodePref, pos, rot);
            var networkObj = obj.GetComponent<NetworkObject>();
            networkObj.DontDestroyWithOwner = true;
            networkObj.Spawn(); 
            networkObj.ChangeOwnership(ownerClientId);
        }
    }
}