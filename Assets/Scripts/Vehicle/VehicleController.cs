using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicle
{
    public class VehicleController : NetworkBehaviour, IVehicleController
    {
        [SerializeField] private PlayerInput _input;

        protected bool IsMoving { get; set; }
        public bool EngineTurnedOn { get; set; }
        protected Vector2 MoveInput;

        public virtual bool EnoughFuel()
            => false;

        public virtual bool HasEngine()
            => false;

        public virtual void TurnOff()
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            IsMoving = true;
            MoveInput = context.ReadValue<Vector2>();
        }

        public void ActivateInput(bool value)
            => _input.enabled = value;

        #region IVehicleController

        public virtual bool CanBePushed()
            => false;

        public virtual void Push(PlayerNetCode playerNetCode)
        {
            throw new System.NotImplementedException();
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