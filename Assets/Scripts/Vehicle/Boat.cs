using System;
using UnityEngine;

namespace Vehicle
{
    public class Boat : BaseVehicle
    {
        [SerializeField] private float _boatMoveSpeed = 5f;
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
            Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
            Debug.LogError(movement);
            movement = transform.TransformDirection(movement) * _boatMoveSpeed * Time.deltaTime;
            VehicleRb.MovePosition(transform.position + movement);
        }

        public Transform SitAtPlace()
        {
            return _sitPlace;
        }
    }
}