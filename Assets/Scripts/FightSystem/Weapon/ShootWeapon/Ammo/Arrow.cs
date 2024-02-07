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
        [SerializeField] private float _torque = 10f;
 
        private void Start()
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody>();
            // StartCoroutine(DespawnObject());
        }
        
        public void ArrowFly(Vector3 force)
        {
            _rb.isKinematic = false;
            _rb.AddForce(force, ForceMode.Impulse);
            _rb.useGravity = true;
            _rb.AddTorque(_rb.transform.forward * _torque);
        }

        private void OnCollisionEnter(Collision other)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            if (IsServer)
            {
                _targetLootingItem.InitByTargetItem();
                GetComponent<NetworkObject>().TrySetParent(other.transform);
            }
               
        }
    }
}