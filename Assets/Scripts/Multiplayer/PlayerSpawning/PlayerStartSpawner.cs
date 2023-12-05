using Building_System.NetWorking;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using Web.User;

namespace Multiplayer.PlayerSpawning
{
    public class PlayerStartSpawner : NetworkBehaviour
    {
        private GameObject _canvas;
       [SerializeField] private PlayerNetCode _playerNetPref;
        
        
        private void Start()
        {
            if(!IsOwner) return;
            PlayerStaffSpawner.Singleton.SpawnPlayerServerRpc(Vector3.zero, Quaternion.identity, UserDataHandler.singleton.UserData.Id, GetComponent<NetworkObject>().OwnerClientId);
            _canvas = GameObject.FindGameObjectWithTag("Canvas");
        }

        public override void OnNetworkDespawn()
        {
            Debug.Log("Despawned");
        }
    }
}