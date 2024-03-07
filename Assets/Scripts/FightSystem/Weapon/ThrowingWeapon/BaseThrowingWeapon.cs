using System;
using FightSystem.Weapon.Ballistic;
using Items_System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace FightSystem.Weapon.ThrowingWeapon
{
    public abstract class BaseThrowingWeapon : NetworkBehaviour
    {
        [FormerlySerializedAs("_targetLootingItem")]
        [Header("Base Throwing Weapon")]
        [SerializeField] protected LootingItem TargetLootingItem;
        [FormerlySerializedAs("_rb")] [SerializeField] protected Rigidbody Rb;
        [SerializeField] private float _speed = 10f;
        
        protected BallisticCalculator _ballisticCalculator = new();

        protected virtual void Start()
        {
            if (Rb == null)
                Rb = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            var velocity = Rb.velocity;
            if (velocity.magnitude > 0.01f)
                gameObject.transform.forward = Rb.velocity;
        }

        public virtual void Throw(float angle, int throwingHp = -1)
        {
            var velocity = _ballisticCalculator.GetCalculatedVelocity(transform.forward, angle, _speed);
            var tipObjRb = gameObject.GetComponentInChildren<Rigidbody>();
            Rb.isKinematic = false;
            Rb.velocity = velocity;
            tipObjRb.useGravity = true;
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
                return;
            }

            Rb.isKinematic = true;
            Rb.constraints = RigidbodyConstraints.FreezeAll;
            if (IsServer)
            {
                TargetLootingItem.InitByTargetItem();
                GetComponent<NetworkObject>().TrySetParent(other.transform);
            }
        }
    }
}