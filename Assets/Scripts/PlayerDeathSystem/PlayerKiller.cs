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
        [SerializeField] private CharacterAnimationsHandler _inventoryAnimationsHandler;

        private AnimationsManager _animationsManager;
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
            int corpesid = Random.Range(0, 100000);
            DieClientRpc(corpesid, wasDisconnected, ownerId, shouldDisplayDeathScreen);
        }

        [ServerRpc(RequireOwnership = false)]
        public void DisconnectServerRpc(int ownerId)
        {
            if (!IsServer) return;
            int corpesid = Random.Range(0, 100000);
            GenerateCorp(corpesid, true, ownerId);
        }
        
        public void AssignAnimationsManager(AnimationsManager animationsManager)
        {
            _animationsManager = animationsManager;
        }

        private void GenerateCorp(int corpesid, bool wasDisconnected, int ownerId)
        {
            _playerCorpesHanler.ResetCorpesPos(corpesid);
            _characterAnimationsHandler.SetAnimationServerRpc(_characterAnimationsHandler.GetAnimationNum("Dead"));
            var networkObject = GetComponent<NetworkObject>();
            networkObject.ChangeOwnership(PlayerNetCode.Singleton.OwnerClientId);
            _playerCorpesHanler.GenerateBackPack(_characterInventory.ItemsNetData.Value, corpesid, wasDisconnected,
                ownerId);
            networkObject.Despawn();
        }

        [ClientRpc]
        private void DieClientRpc(int corpesid, bool wasDisconnected, int ownerId, bool shouldDisplayDeathScreen)
        {
            _playerCorpesHanler.ResetCorpesPos(corpesid);
            if (ownerId == _userId.Value && IsOwner)
            {
                if (shouldDisplayDeathScreen)
                    MainUiHandler.Singleton.DisplayDeathScreen(true);
                AnimationsManager.Singleton.SetDeath();
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
            MainUiHandler.Singleton.DisplayKnockDownScreen(false);
            DieServerRpc(UserDataHandler.Singleton.UserData.Id, false);
        }
    }
}