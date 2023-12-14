using System.Linq;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace Vehicle.SittingPlaces
{
    public abstract class SittingPlace : NetworkBehaviour
    {
        [SerializeField] private NetworkObject _networkObject;
        [SerializeField] private Transform _standingPoint;
        private PlayerNetCode _currentPlayer;

        private void Awake()
            => gameObject.tag = "SitPlace";

        #region SitIn

        public bool CanSit() => _currentPlayer == null;

        [ClientRpc]
        private void AssignCurrentPlayerClientRpc(ulong playerId)
        {
            var players = FindObjectsOfType<PlayerNetCode>().ToList();
            foreach (var player in players)
            {
                if (player.NetworkObjectId != playerId) continue;
                _currentPlayer = player;
            }
        }

        private void SetPlayer(ulong networkId)
        {
            if (!IsServer) return;
            var players = FindObjectsOfType<PlayerNetCode>().ToList();
            foreach (var player in players)
            {
                var networkObject = player.GetComponent<NetworkObject>();
                if (networkObject.NetworkObjectId != networkId) continue;
                networkObject.TrySetParent(_networkObject.transform);
                player.ChangePositionClientRpc(transform.position);
                AssignCurrentPlayerClientRpc(networkId);
                return;
            }
        }


        [ServerRpc(RequireOwnership = false)]
        private void SitServerRpc(ulong playerId)
        {
            if (!IsServer) return;
            SetPlayer(playerId);
            _currentPlayer.SitClientRpc();
        }

        [ClientRpc]
        private void ResetPlayerClientRpc()
            => _currentPlayer = null;

        public virtual void SitIn(PlayerNetCode playerNetCode)
            => SitServerRpc(playerNetCode.NetworkObjectId);

        #endregion

        #region StandUp

        public bool CanStand(PlayerNetCode playerNetCode)
            => playerNetCode.NetworkObjectId == _currentPlayer.NetworkObjectId;

        private void ResetPlayer()
        {
            if (_currentPlayer == null) return;
            _currentPlayer.GetComponent<NetworkObject>().TryRemoveParent();
            _currentPlayer.ChangePositionClientRpc(_standingPoint.position);
            ResetPlayerClientRpc();
        }
        
        protected virtual void ResetInput(){}

        [ServerRpc(RequireOwnership = false)]
        private void StandUpServerRpc(ulong playerId)
        {
            if (!IsServer) return;
            _currentPlayer.StandClientRpc();
            ResetPlayer();
        }

        public virtual void StandUp(PlayerNetCode playerNetCode)
            => StandUpServerRpc(playerNetCode.NetworkObjectId);

        #endregion

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (_currentPlayer == null) return;
            if (_currentPlayer.NetworkObjectId != other.GetComponent<PlayerNetCode>().NetworkObjectId) return;
            ResetPlayerClientRpc();
            ResetInput();
        }
    }
}