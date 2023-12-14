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
        [SerializeField] private PlayerInput _input;

        protected bool IsMoving { get; set; }

        protected Vector2 MoveInput;

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