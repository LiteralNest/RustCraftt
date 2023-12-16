using System.Collections.Generic;
using System.Threading.Tasks;
using Animation_System;
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

        private NetworkVariable<int> _userId = new(-1, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public int UserId => _userId.Value;

        private async void Start()
        {
            await Task.Delay(1000);
            if (!IsOwner) return;
            Singleton = this;
            _userId.Value = UserDataHandler.singleton.UserData.Id;
        }

        [ServerRpc(RequireOwnership = false)]
        public void DieServerRpc(int ownerId, bool wasDisconnected = false, bool shouldDisplayDeathScreen = true)
        {
            if (!IsServer) return;
            int corpesid = Random.Range(0, 100000);
            DieClientRpc(corpesid, wasDisconnected, ownerId, shouldDisplayDeathScreen);
        }

        [ClientRpc]
        private void DieClientRpc(int corpesid, bool wasDisconnected, int ownerId, bool shouldDisplayDeathScreen)
        {
            _playerCorpesHanler.ResetCorpesPos(corpesid);
            if (ownerId == _userId.Value)
            {
                if(shouldDisplayDeathScreen)
                    MainUiHandler.Singleton.DisplayDeathScreen(true);
                AnimationsManager.Singleton.SetDeath(false);
            }

            if (IsServer)
            {
                GetComponent<NetworkObject>().ChangeOwnership(PlayerNetCode.Singleton.OwnerClientId);
                _playerCorpesHanler.GenerateBackPack(_characterInventory.ItemsNetData.Value, corpesid, wasDisconnected,
                    ownerId);
                GetComponent<NetworkObject>().Despawn();
            }
        }

        [ContextMenu("Die")]
        private void Die()
        {
            DieServerRpc(UserDataHandler.singleton.UserData.Id, false);
        }
    }
}