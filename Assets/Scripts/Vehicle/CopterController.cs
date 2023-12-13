using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicle
{
    public class CopterController : VehicleController
    {
        [SerializeField] private Copter _copter;

        private void Update()
        {
            if (MoveInput.x != 0) return;
            _copter.Stabilize();
        }

        private void FixedUpdate()
        {
            if (!IsMoving) return;
            _copter.Move(MoveInput);
        }

        public override bool CanMoveUp()
            => true;

        public override void HandleMovingDown(bool value)
            => _copter.MovingDown = value;

        public override bool CanMoveDown()
            => true;

        public override void StandUp(PlayerNetCode playerNetCode)
        {
            base.StandUp(playerNetCode);
            _copter.ReturnKinematic();
        }

        public override void HandleMovingUp(bool value)
            => _copter.MovingUp = value;
    }
}