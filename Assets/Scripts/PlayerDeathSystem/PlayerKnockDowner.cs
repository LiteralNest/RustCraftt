using Animation_System;
using Character_Stats;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem
{
    public class PlayerKnockDowner : NetworkBehaviour
    {
        public static PlayerKnockDowner Singleton { get; private set; }

        [SerializeField] private PlayerCorpesHanler _playerCorpesHanler;
        [SerializeField] private CharacterAnimationsHandler _characterAnimationsHandler;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner) return;
            Singleton = this;
        }

        #region KnockDown

        [ServerRpc(RequireOwnership = false)]
        public void KnockDownServerRpc()
        {
            if (!IsServer) return;
            KnockDownClientRpc();
        }

        [ClientRpc]
        private void KnockDownClientRpc()
        {
            if (IsOwner)
                MainUiHandler.Singleton.DisplayKnockDownScreen(true);
            _characterAnimationsHandler.SetKnockDown();
        }

        [ContextMenu("KnockDown")]
        private void KnockDown() 
        {
            KnockDownServerRpc();
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
            _characterAnimationsHandler.SetIdle();
            if (IsOwner) 
            {
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