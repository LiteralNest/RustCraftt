using System;
using UnityEngine;

namespace Vehicle
{
    public class Horse : BaseVehicle
    {
        [SerializeField] private float _galopingSpeed = 10f;
        [SerializeField]private float _jumpForce = 0.5f;
        private float _currentMovingSpeed;

        private void Start()
        {
            _currentMovingSpeed = MoveSpeed;
        }

        public void Move(Vector2 moveInput)
        {
            Debug.Log("Move method called"); 
            float forwardMovement = Mathf.Clamp(moveInput.y, 0, 1f);
            var movement = new Vector3(0f, 0f, forwardMovement);
            var rotation = moveInput.x * RotationSpeed * Time.fixedDeltaTime;

            transform.Rotate(0f, rotation, 0f);

            if (forwardMovement > 0)
            {
                transform.Translate(movement * MoveSpeed * Time.deltaTime, Space.Self);
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
            VehicleRb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
}