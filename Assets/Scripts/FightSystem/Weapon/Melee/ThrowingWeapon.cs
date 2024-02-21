using System.Collections;
using Items_System;
using Items_System.Items;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Weapon.Melee
{
    public class ThrowingWeapon : NetworkBehaviour
    {
        [Header("Attached Compontents")] 
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private LootingItem _lootingItem;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private ForceMode _forceMode;

        private Collision _hitObject;

        [Header("Main Params")] [SerializeField]
        private float _lerpSpeed = 2f;

        private int _throwingHp;

        private void Start()
        {
            int arrowLayer = LayerMask.NameToLayer("Arrow");
            Physics.IgnoreLayerCollision(arrowLayer, arrowLayer);
            StartCoroutine(WaitForEnable());
        }

        private IEnumerator WaitForEnable()
        {
            _collider.enabled = false;
            yield return new WaitForSeconds(0.05f);
            _collider.enabled = true;
        }

        public void Throw(Vector3 direction, float force, int throwingHp)
        {
            _throwingHp = throwingHp;
            if (!_rb) return;
            _rb.AddForce(direction * force, _forceMode);
            Rotate();
        }

        private void Rotate()
        {
            if (!_rb) return;
            var velocity = _rb.velocity.normalized;
            if (_rb.velocity.sqrMagnitude > 0.01f)
            {
                var newRotation = Quaternion.LookRotation(velocity, Vector3.down);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * _lerpSpeed);
            }
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

        private void OnCollisionEnter(Collision other)
        {
            if (!IsServer) return;
            if (!_rb) return;
            MinusItemHp();
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            if(_hitObject == null)
                _hitObject = other;
            StartCoroutine(CheckHitObject());
        }
    }
}