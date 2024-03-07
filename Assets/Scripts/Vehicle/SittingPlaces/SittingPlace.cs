using System.Linq;
using Animation_System;
using InteractSystem;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace Vehicle.SittingPlaces
{
    public abstract class SittingPlace : NetworkBehaviour, IRaycastInteractable
    {
        [SerializeField] private Sprite _displayIcon;
        [SerializeField] private NetworkObject _networkObject;
        [SerializeField] private Transform _standingPoint;
        private PlayerNetCode _currentPlayer;

        private void Awake()
            => gameObject.tag = "SitPlace";

        #region IRaycastInteractable

        public bool CanDisplayInteract()
            => true;
        
        public string GetDisplayText()
        {
            if (CanSit())
                return "Sit";
            if (CanStand(PlayerNetCode.Singleton))
                return "Stand";
            
            return "";
        }

        public void Interact()
        {
            if(CanSit())
                SitIn(PlayerNetCode.Singleton);
            else if(CanStand(PlayerNetCode.Singleton))
                StandUp(PlayerNetCode.Singleton);
        }

        public Sprite GetIcon()
            => _displayIcon;

        public bool CanInteract()
        {
            if (CanSit() || CanStand(PlayerNetCode.Singleton)) return true;
            return false;
        }

        #endregion

        #region SitIn

        public bool CanSit() => _currentPlayer == null;

        [ClientRpc]
        private void AssignCurrentPlayerClientRpc(ulong playerId, ulong ownerId)
            => AssignCurrentPlayer(playerId, ownerId);

        private void AssignCurrentPlayer(ulong playerId, ulong ownerId)
        {
            var players = FindObjectsOfType<PlayerNetCode>().ToList();
            foreach (var player in players)
            {
                if (player.NetworkObjectId != ownerId) continue;

                _currentPlayer = player;
            }
        }

        private void SetPlayer(ulong networkId, ulong ownerId)
        {
            if (!IsServer) return;
            var players = FindObjectsOfType<PlayerNetCode>().ToList();
            foreach (var player in players)
            {
                var networkObject = player.GetComponent<NetworkObject>();
                if (networkObject.OwnerClientId != networkId) continue;
                networkObject.TrySetParent(_networkObject.transform);
                player.ChangePositionClientRpc(transform.position);
                AssignCurrentPlayer(networkId, ownerId);
                AssignCurrentPlayerClientRpc(networkId, ownerId);
                return;
            }
        }


        [ServerRpc(RequireOwnership = false)]
        private void SitServerRpc(ulong playerId, ulong ownerId)
        {
            if (!IsServer) return;
            SetPlayer(playerId, ownerId);
            _currentPlayer.SitClientRpc();
            _networkObject.ChangeOwnership(playerId);
            _networkObject.DontDestroyWithOwner = true;
        }

        [ClientRpc]
        private void ResetPlayerClientRpc()
            => _currentPlayer = null;

        protected virtual void SitIn(PlayerNetCode playerNetCode)
        {
            AnimationsManager.Singleton.SetSit();
            SitServerRpc(playerNetCode.OwnerClientId, playerNetCode.NetworkObjectId);
        }

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

        protected virtual void ResetInput()
        {
        }

        [ServerRpc(RequireOwnership = false)]
        private void StandUpServerRpc(ulong playerId)
        {
            if (!IsServer) return;
            _currentPlayer.StandClientRpc();
            ResetPlayer();
        }

        protected virtual void StandUp(PlayerNetCode playerNetCode)
        {
            AnimationsManager.Singleton.SetSit();
            StandUpServerRpc(playerNetCode.NetworkObjectId);
        }

        #endregion

        protected virtual void TriggerExited(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (_currentPlayer == null) return;
            if (_currentPlayer.NetworkObjectId != other.GetComponent<PlayerNetCode>().NetworkObjectId) return;
            ResetPlayerClientRpc();
            ResetInput();
        }

        private void OnTriggerExit(Collider other)
            => TriggerExited(other);
    }
}