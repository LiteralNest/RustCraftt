using System.Threading.Tasks;
using Animation_System;
using Character_Stats;
using Player_Controller;
using UI;
using Unity.Netcode;
using UnityEngine;
using Web.User;

namespace PlayerDeathSystem
{
    public class PlayerKnockDowner : NetworkBehaviour
    {
        public static PlayerKnockDowner Singleton { get; private set; }

        [SerializeField] private CharacterHpHandler _characterHpHandler;
        [SerializeField] private CharacterAnimationsHandler _characterAnimationsHandler;

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
            KnockDownClientRpc(id);
        }

        [ClientRpc]
        private void KnockDownClientRpc(int id)
        {
            if (UserDataHandler.singleton.UserData.Id == id)
            {
                GetComponent<PlayerController>().enabled = false;
                MainUiHandler.Singleton.DisplayKnockDownScreen(true);
                var item = InventoryHandler.singleton.ActiveItem;
                if (item == null) return;
                InstantiatingItemsPool.sigleton.SpawnDropableObjectServerRpc(item.Id, 1, Camera.main.transform.position + Camera.main.transform.forward);
                PlayerNetCode.Singleton.InHandObjectsContainer.SetDefaultHands();
                AnimationsManager.Singleton.SetKnockDown();
            }
        }

        [ContextMenu("KnockDown")]
        private void KnockDown()
        {
            KnockDownServerRpc(UserDataHandler.singleton.UserData.Id);
        }

        #endregion

        #region StandUp

        [ServerRpc(RequireOwnership = false)]
        public void StandUpServerRpc()
        {
            if (!IsServer) return;
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