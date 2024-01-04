using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicle
{
    public class HorseController : VehicleController, IVehicleController
    {
        [SerializeField] private Horse _horse;
        
        private RigidbodyConstraints _rbConstraints;
        
        private void FixedUpdate()
        {
            if (IsMoving)
            {
                _horse.Move(MoveInput);
            }
        }
    }
}