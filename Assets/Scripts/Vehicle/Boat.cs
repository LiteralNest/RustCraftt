using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Vehicle
{
    public class Boat : BaseVehicle
    {
        [SerializeField] private float _pushForce;
        [SerializeField] private float _depthBeforeSubmerged;
        [SerializeField] private float _displacementAmount;

        public void BoatFloating(float waveHeight)
        {
            if (transform.position.y < waveHeight)
            {
                var displacementMultiplier = Mathf.Clamp01(waveHeight - transform.position.y / _depthBeforeSubmerged) * _displacementAmount;
                VehicleRb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("1121");
            var player = collision.gameObject.CompareTag("Player");
            if (player)
            {
                PushVehicle(collision);
            }
        }

        private void PushVehicle(Collision collision)
        {
            var playerRb = collision.gameObject.GetComponent<Rigidbody>();
    
            var pushDirection = (transform.position - collision.transform.position).normalized;
            
            var pushForce = pushDirection * _pushForce;
    
            playerRb.AddForce(pushForce, ForceMode.Impulse);
        }
    }
}