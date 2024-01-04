using UnityEngine;

namespace Vehicle
{
    public class CopterController : VehicleController
    {
        [SerializeField] private Copter _copter;

        public override bool EnoughFuel()
            => _copter.FuelStorage.FuelAvailable();
        
        public override bool HasEngine()
            => true;
        
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

        public override void TurnOff()
        {
            _copter.ReturnKinematic();
        }
        
        public override bool CanMoveUp()
            => true;

        public override void HandleMovingDown(bool value)
            => _copter.MovingDown = value;

        public override bool CanMoveDown()
            => true;

        public override void HandleMovingUp(bool value)
            => _copter.MovingUp = value;
    }
}