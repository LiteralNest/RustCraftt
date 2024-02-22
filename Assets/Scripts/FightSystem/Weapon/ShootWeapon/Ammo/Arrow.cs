using System;
using System.Collections;
using Items_System;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon.Ammo
{
    public class Arrow : NetworkBehaviour
    {
        [Header("Attached Components")]
        [SerializeField] private LootingItem _targetLootingItem;
        [Header("Main Values")]
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _angleInDegrees = 15f;
        [SerializeField] private float _speed = 10f;

        private void Start()
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody>();
            // StartCoroutine(DespawnObject());
        }

        private void Update()
        {
            var velocity = _rb.velocity;
            if(velocity.magnitude > 0.01f) 
                gameObject.transform.forward = _rb.velocity;
        }

        public void ArrowFly()
        {
            var direction = CalculateDirection();
            var v = CalculateVelocity(direction);
            var tipObjRb = gameObject.GetComponentInChildren<Rigidbody>();
            _rb.isKinematic = false;
            _rb.velocity = v;
            tipObjRb.useGravity = true;
        }


        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
                return;
            }

            _rb.constraints = RigidbodyConstraints.FreezeAll;
            if (IsServer)
            {
                _targetLootingItem.InitByTargetItem();
                GetComponent<NetworkObject>().TrySetParent(other.transform);
            }
               
        }
        
        private Vector3 CalculateDirection()
        {
            return transform.forward;
        }

        private Vector3 CalculateVelocity(Vector3 direction)
        {
            var angleInRadians = _angleInDegrees * Mathf.Deg2Rad;
            var horizontalSpeed = Mathf.Cos(angleInRadians) * _speed;
            var verticalSpeed = Mathf.Sin(angleInRadians) * _speed;

            var velocity = direction.normalized * horizontalSpeed;
            velocity.y = verticalSpeed;

            return velocity;
        }
    }
}