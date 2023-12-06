using UI;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem
{
    public class PlayerKnockDowner : NetworkBehaviour
    {
        public static PlayerKnockDowner Singleton { get; private set; }

        [SerializeField] private PlayerCorpesHanler _playerCorpesHanler;

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
            _playerCorpesHanler.AssignKnockDown();
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
            if (IsOwner) 
            {
                MainUiHandler.Singleton.DisplayKnockDownScreen(false);
                CharacterStats.Singleton.PlusStat(CharacterStatType.Health, 10);
            }
              
            _playerCorpesHanler.ReturnToDefaultPosition();
        }

        [ContextMenu("StandUp")]
        private void StandUp()
        {
            StandUpServerRpc();
        }

        #endregion
    }
}