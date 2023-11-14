using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Vehicle
{
    public class Boat : BaseVehicle
    {
        
        [SerializeField] private float _pushForce = 100f;
        [SerializeField] private float _depthBeforeSubmerged = 1f;
        [SerializeField] private float _displacementAmount = 1.09f;
        

        private float _originalRb = 0;
        public bool IsFloating { get; set; }

        public void SetVehicleRbInWater()
        {
            VehicleRb.drag = 1.04f;
            VehicleRb.mass = 50f;
        }

        private void SetVehicleRbOnGround()
        {
            VehicleRb.drag = _originalRb;
            VehicleRb.mass = 0f;
        }
        public void Float(float waveHeight, bool statement)
        {
            IsFloating = statement;

            if (IsFloating)
            {
                SetVehicleRbInWater();
                if (!(transform.position.y < waveHeight) || !statement) return;
                
                var displacementMultiplier = Mathf.Clamp01(waveHeight - transform.position.y / _depthBeforeSubmerged) * _displacementAmount;
                VehicleRb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
            }
            else SetVehicleRbOnGround();
               
        }
        public void Push(Vector3 pushDirection)
        {
            VehicleRb.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
        }

        public void Move(Vector2 moveInput)
        {
            float forwardMovement = Mathf.Clamp(moveInput.y, 0, 1f);
            var movement = new Vector3(0f, 0f, forwardMovement);
            var rotation = moveInput.x * RotationSpeed * Time.fixedDeltaTime;

            transform.Rotate(0f, rotation, 0f);

            if (forwardMovement > 0)
            {
                transform.Translate(movement * MoveSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}