using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Vehicle
{
    public class HorseController : VehicleController
    {
        [SerializeField] private Horse _horse;
        [SerializeField] private float _obstacleCheckDistance = 2f;
        
        private void Update()
        {
            if (IsMoving)
            {
                _horse.Move(MoveInput);
                CheckForObstacle();
            }
            _horse.UpdateGravity();
        }
        
        private void CheckForObstacle()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, _obstacleCheckDistance, LayerMask.GetMask("Ground")))
            {
                _horse.Jump();
            }
        }
    }
}