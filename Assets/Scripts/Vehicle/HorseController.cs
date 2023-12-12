using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicle
{
    public class HorseController : VehicleController, IVehicleController
    {
        [SerializeField] private Horse _horse;
        [SerializeField] private PlayerInput _horseInput;

        public bool IsMoving { get; set; }

        private Vector2 _moveInput;
        private RigidbodyConstraints _rbConstraints;

        private void Awake()
        {
            _horseInput.enabled = false;
        }

        private void FixedUpdate()
        {
            if (IsMoving)
            {
                _horse.Move(_moveInput);
            }
        }

        #region InputMap

        public void OnMove(InputAction.CallbackContext context)
        {
            IsMoving = true;
            _moveInput = context.ReadValue<Vector2>();
        }

        #endregion


        #region IVehicleController

        public bool CanBePushed()
            => false;

        public void Push(PlayerNetCode playerNetCode)
        {
            throw new System.NotImplementedException();
        }

        public bool CanStand()
            => _currentPlayer != null;

        public void StandUp(PlayerNetCode playerNetCode)
        {
            StandUpServerRpc(playerNetCode.NetworkObjectId);
            _horseInput.enabled = false;
        }

        public bool CanSit()
            => _currentPlayer == null;

        public void SitIn(PlayerNetCode playerNetCode)
        {
            SitServerRpc(playerNetCode.NetworkObjectId);
            _horseInput.enabled = true;
        }

        public bool CanMoveUp()
            => false;

        public void MoveUp(PlayerNetCode playerNetCode)
        {
            throw new System.NotImplementedException();
        }

        public bool CanMoveDown()
            => false;

        public void MoveDown(PlayerNetCode playerNetCode)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}