using System.Linq;
using System.Threading.Tasks;
using Building_System.NetWorking;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicle
{
    public class VehicleController : NetworkBehaviour, IVehicleController
    {
        [SerializeField] protected Vector3 _offset = new Vector3(0f, 3f, 0f);
        protected PlayerNetCode _currentPlayer;
        [SerializeField] private PlayerInput _input;

        protected bool IsMoving { get; set; }

        protected Vector2 MoveInput;

        public void OnMove(InputAction.CallbackContext context)
        {
            IsMoving = true;
            MoveInput = context.ReadValue<Vector2>();
        }

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
                networkObject.TrySetParent(this.transform);
                GetComponent<NetworkObject>().ChangeOwnership(networkObject.OwnerClientId);
                GetComponent<NetworkObject>().DontDestroyWithOwner = true;
                player.ChangePositionClientRpc(transform.position + _offset);
                AssignCurrentPlayerClientRpc(networkId);
                return;
            }
        }

        private void ResetPlayer()
        {
            if (_currentPlayer == null) return;
            _currentPlayer.GetComponent<NetworkObject>().TryRemoveParent();
            //ResetPlayerClientRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void StandUpServerRpc(ulong playerId)
        {
            if (!IsServer) return;
            _currentPlayer.StandClientRpc();
            ResetPlayer();
        }

        #region IVehicleController

        public virtual bool CanBePushed()
            => false;

        public virtual void Push(PlayerNetCode playerNetCode)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool CanStand()
            => _currentPlayer != null;

        public virtual void StandUp(PlayerNetCode playerNetCode)
        {
            StandUpServerRpc(playerNetCode.NetworkObjectId);
            _input.enabled = false;
        }

        public virtual bool CanSit()
            => _currentPlayer == null;

        public virtual void SitIn(PlayerNetCode playerNetCode)
        {
            //SitServerRpc(playerNetCode.NetworkObjectId);
            _input.enabled = true;
        }

        public virtual bool CanMoveUp()
            => false;

        public virtual void HandleMovingDown(bool value)
        {
        }

        public virtual bool CanMoveDown()
            => false;

        public virtual void HandleMovingUp(bool value)
        {
        }

        #endregion
    }
}