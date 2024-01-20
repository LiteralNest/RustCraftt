using System;
using UnityEngine;

namespace Vehicle
{
    public class Horse : BaseVehicle
    {
        [SerializeField] private float _galopingSpeed = 10f;
        [SerializeField] private float _jumpForce = 20f;
        private float _currentMovingSpeed;
        private Vector3 _velocity;

        private void Start()
        {
            _currentMovingSpeed = MoveSpeed;
        }

        public void Move(Vector2 moveInput)
        {
            float forwardMovement = Mathf.Clamp(moveInput.y, 0, 1f);
            var movement = new Vector3(0f, 0f, forwardMovement);
            var rotation = moveInput.x * RotationSpeed * Time.fixedDeltaTime;

            transform.Rotate(0f, rotation, 0f);

            if (forwardMovement > 0)
            {
                VehicleController.Move(transform.TransformDirection(movement * MoveSpeed * Time.deltaTime));
            }
        }

        public void StartRun()
        {
            _currentMovingSpeed *= _galopingSpeed;
        }

        public void StopRun()
        {
            MoveSpeed = _currentMovingSpeed;
        }
        public void Jump()
        {
            if (VehicleController.isGrounded)
            {
                _velocity.y = -1f;
            }
            else
            {
                _velocity.y = Mathf.Sqrt(_jumpForce * -2f * Gravity);
            }
        }
        
        public void UpdateGravity()
        {
            if (VehicleController.isGrounded) _velocity.y = -1f;
            else _velocity.y += Gravity * Time.deltaTime;


            VehicleController.Move(_velocity * Time.deltaTime);
        }
    }
}