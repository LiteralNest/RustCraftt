using System.Threading.Tasks;
using Animation_System;
using Player_Controller;
using Storage_System;
using UI;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace PlayerDeathSystem
{
    public class PlayerKiller : NetworkBehaviour
    {
        public static PlayerKiller Singleton { get; private set; }

        [SerializeField] private CharacterInventory _characterInventory;
        [SerializeField] private PlayerCorpesHanler _playerCorpesHanler;
        [SerializeField] private CharacterAnimationsHandler _characterAnimationsHandler;

        private NetworkVariable<int> _userId = new(-1, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public int UserId => _userId.Value;

        private async void Start()
        {
            await Task.Delay(1000);
            if (!IsOwner) return;
            Singleton = this;
            _userId.Value = UserDataHandler.Singleton.UserData.Id;
        }

        [ServerRpc(RequireOwnership = false)]
        public void DieServerRpc(int ownerId, bool wasDisconnected = false, bool shouldDisplayDeathScreen = true)
        {
            if (!IsServer) return;
            DieClientRpc(wasDisconnected, ownerId, shouldDisplayDeathScreen);
        }

        [ServerRpc(RequireOwnership = false)]
        public void GenerateBackPackServerRpc(bool wasDisconnected, int ownerId, string nickName)
        {
            if(!IsServer) return;
            GenerateBackPack(wasDisconnected, ownerId, nickName);
        }

        public void GenerateBackPack(bool wasDisconnected, int ownerId, string nickName)
        {
            GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
            _playerCorpesHanler.GenerateBackPack(_characterInventory.ItemsNetData.Value, wasDisconnected,
                ownerId, nickName);
            GetComponent<NetworkObject>().Despawn();
        }
        

        [ClientRpc]
        private void DieClientRpc(bool wasDisconnected, int ownerId, bool shouldDisplayDeathScreen)
        {
            if (ownerId == _userId.Value && IsOwner)
            {
                if (shouldDisplayDeathScreen)
                    MainUiHandler.Singleton.DisplayDeathScreen(true);
                GenerateBackPackServerRpc(wasDisconnected, ownerId, UserDataHandler.Singleton.UserData.Name);
            }
        }

        [ContextMenu("Die")]
        private void Die()
        {
            MainUiHandler.Singleton.DisplayKnockDownScreen(false);
            DieServerRpc(UserDataHandler.Singleton.UserData.Id, false);
        }
    }
}