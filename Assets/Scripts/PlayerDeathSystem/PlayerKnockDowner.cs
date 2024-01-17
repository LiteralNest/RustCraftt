using System.Threading.Tasks;
using Animation_System;
using Character_Stats;
using Player_Controller;
using Player_Controller.Looking_Around;
using UI;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace PlayerDeathSystem
{
    public class PlayerKnockDowner : NetworkBehaviour
    {
        public static PlayerKnockDowner Singleton { get; private set; }

        [SerializeField] private CharacterHpHandler _characterHpHandler;
        
        [Header("Head")]
        [SerializeField] private PlayerRotator _playerRotator;

        
        private NetworkVariable<bool> _knockDown = new();
        public bool IsKnockDown => _knockDown.Value;

        private async void Start()
        {
            await Task.Delay(1000);
            if (!IsOwner) return;
            Singleton = this;
        }

        #region KnockDown

        [ServerRpc(RequireOwnership = false)]
        public void KnockDownServerRpc(int id)
        {
            if (!IsServer) return;
            _knockDown.Value = true;
            KnockDownClientRpc(id);
        }

        [ClientRpc]
        private void KnockDownClientRpc(int id)
        {
            if (UserDataHandler.Singleton.UserData.Id == id)
            {
                GetComponent<PlayerController>().enabled = false;
                MainUiHandler.Singleton.DisplayKnockDownScreen(true);
                var item = InventoryHandler.singleton.ActiveItem;
                if (item != null)
                {
                    InstantiatingItemsPool.sigleton.SpawnDropableObjectServerRpc(item.Id, 1, Camera.main.transform.position + Camera.main.transform.forward);
                    InventoryHandler.singleton.CharacterInventory.RemoveItem(item.Id, 1);
                    PlayerNetCode.Singleton.InHandObjectsContainer.SetDefaultHands();
                }
                AnimationsManager.Singleton.SetKnockDown();
            }
        }

        [ContextMenu("KnockDown")]
        private void KnockDown()
        {
            KnockDownServerRpc(UserDataHandler.Singleton.UserData.Id);
        }

        #endregion

        #region StandUp

        [ServerRpc(RequireOwnership = false)]
        public void StandUpServerRpc()
        {
            if (!IsServer) return;
            _knockDown.Value = false;
            StandUpClientRpc();
        }

        [ClientRpc]
        private void StandUpClientRpc()
        {
            AnimationsManager.Singleton.SetIdle();
            if (IsOwner)
            {
                _characterHpHandler.SetKnockedDownServerRpc(false);
                GetComponent<PlayerController>().enabled = true;
                MainUiHandler.Singleton.DisplayKnockDownScreen(false);
                CharacterStats.Singleton.PlusStat(CharacterStatType.Health, 10);
            }
        }

        [ContextMenu("StandUp")]
        private void StandUp()
        {
            StandUpServerRpc();
        }

        #endregion
    }
}