using System;
using UnityEngine;

namespace Vehicle
{
    public class Boat : BaseVehicle
    {
        [SerializeField] private float _boatMoveSpeed = 5f;
        [SerializeField] private float _boatRotationSpeed = 10f;
        [SerializeField] private float _pushForce = 10f;
        [SerializeField] private float _depthBeforeSubmerged = 1f;
        [SerializeField] private float _displacementAmount = 2f;
        [SerializeField] private Transform _sitPlace;
        
        
        public void Float(float waveHeight, bool statement)
        {
            if (transform.position.y < waveHeight && statement)
            {
                var displacementMultiplier = Mathf.Clamp01(waveHeight - transform.position.y / _depthBeforeSubmerged) * _displacementAmount;
                VehicleRb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
            }
        }
        public void Push(Vector3 pushDirection)
        {
            VehicleRb.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
        }

        public void Move(Vector2 moveInput)
        {
            float forwardMovement = Mathf.Clamp(moveInput.y, 0, 1f);
            var movement = new Vector3(forwardMovement, 0f, 0f);
            var rotation = moveInput.x * _boatRotationSpeed * Time.fixedDeltaTime;

            transform.Rotate(0f, rotation, 0f);

            if (forwardMovement > 0)
            {
                transform.Translate(-movement * _boatMoveSpeed * Time.deltaTime, Space.Self);
            }
        }

        public Transform SitAtPlace()
        {
            return _sitPlace;
        }
    }
}