using System.Collections;
using Items_System;
using Items_System.Items;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Weapon.Melee
{
    public class ThrowingWeapon : NetworkBehaviour
    {
        [Header("Attached Components")] 
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private LootingItem _lootingItem;
        [SerializeField] private BoxCollider _collider;
        [Header("Main Params")]
        [SerializeField] private float _lerpSpeed = 2f;
        [SerializeField] private float _angleInDegrees;
        [SerializeField] private float _speed;
        
        private int _throwingHp;
        private Collision _hitObject;

        private void Start()
        {
            // var arrowLayer = LayerMask.NameToLayer("Arrow");
            // Physics.IgnoreLayerCollision(arrowLayer, arrowLayer);
            StartCoroutine(WaitForEnable());
        }

        private IEnumerator WaitForEnable()
        {
            _collider.enabled = false;
            yield return new WaitForSeconds(0.05f);
            _collider.enabled = true;
        }

        private void Update()
        {
            if (!_rb) return;
            var velocity = _rb.velocity.normalized;
            if (_rb.velocity.sqrMagnitude > 0.01f)
            {
                var newRotation = Quaternion.LookRotation(velocity, Vector3.down);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * _lerpSpeed);
            }
        }

        public void Throw(int throwingHp)
        {
            _throwingHp = throwingHp;
            if (!_rb) return;
            
            var direction = CalculateDirection();
            var v = CalculateVelocity(direction);

           
            var tipObjRb = gameObject.GetComponentInChildren<Rigidbody>();
            _rb.isKinematic = false;
            _rb.velocity = v;
            tipObjRb.useGravity = true;
        }

        private void MinusItemHp()
        {
            var item = _lootingItem.TargetItem as DamagableItem;
            var minusingHp = item.Hp / 10;
            var currentHp = _throwingHp - minusingHp;
            _lootingItem.InitByTargetItem(currentHp, currentHp <= 0);
        }

        private IEnumerator CheckHitObject()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (_hitObject == null || _hitObject.collider.enabled == false)
                    _rb.constraints = RigidbodyConstraints.None;
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
        
        private void OnCollisionEnter(Collision other)
        {
            _rb.isKinematic = true;
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            if (!IsServer) return;
            if (!_rb) return;
            MinusItemHp();
            if (other.gameObject.CompareTag("Arrow"))
            {
                Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
                return;
            }
            
            if(_hitObject == null)
                _hitObject = other;
            StartCoroutine(CheckHitObject());
        }
    }
}