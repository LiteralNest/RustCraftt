using Player_Controller;
using UnityEngine;

namespace Vehicle.Boat
{
    public class BoatController : VehicleController
    {
        [SerializeField] private Boat _boat;

        public override bool EnoughFuel()
            => _boat.FuelStorage.FuelAvailable();

        public override bool HasEngine()
            => true;

        private void FixedUpdate()
        {
            if (IsMoving && _boat.IsFloating)
            {
                _boat.Move(MoveInput);
            }
        }

        public override bool CanBePushed()
            => true;

        public override void Push(PlayerNetCode playerNetCode)
        {
            var player = playerNetCode;
            if (player != null)
            {
                Vector3 pushDirection = (player.transform.forward).normalized;

                _boat.Push(pushDirection);
            }
        }
    }
}