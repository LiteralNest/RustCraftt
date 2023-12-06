using System.Collections.Generic;
using Player_Controller;
using Storage_System;
using UI;
using Unity.Netcode;
using UnityEngine;
using Web.User;

namespace PlayerDeathSystem
{
    public class PlayerKiller : NetworkBehaviour
    {
        public static PlayerKiller Singleton { get; private set; }
        
        [SerializeField] private CharacterInventory _characterInventory;
        [SerializeField] private List<Behaviour> _removingComponents;
        [SerializeField] private List<GameObject> _removingObjects;

        [SerializeField] private PlayerCorpesHanler _playerCorpesHanler; 

        private NetworkVariable<int> _userId = new(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public int UserId => _userId.Value;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(!IsOwner) return;
            Singleton = this;
        }
        
        private void Start()
        {
            if (!IsOwner) return;
            _userId.Value = UserDataHandler.singleton.UserData.Id;
        }

        [ServerRpc(RequireOwnership = false)]
        public void RemoveMainComponentsServerRpc()
        {
            if(!IsServer) return;
            DieClientRpc();
        }

        [ClientRpc]
        private void DieClientRpc()
        {
            _playerCorpesHanler.AssignCorpes(_characterInventory.ItemsNetData.Value);
            if(IsOwner)
                MainUiHandler.Singleton.DisplayDeathScreen(true);
            
            if(IsServer)
                GetComponent<NetworkObject>().ChangeOwnership(PlayerNetCode.Singleton.OwnerClientId);

            foreach (var component in _removingComponents)
            {
                if (component == null) continue;
                Destroy(component);
            }

            foreach (var obj in _removingObjects)
            {
                if (obj == null) continue;
                Destroy(obj);
            }
        }

        [ContextMenu("Die")]
        private void Die()
        {
            RemoveMainComponentsServerRpc();
        }
    }
}
