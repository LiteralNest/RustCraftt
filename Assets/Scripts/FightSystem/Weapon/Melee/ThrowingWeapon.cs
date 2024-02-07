using Items_System;
using Items_System.Items;
using Player_Controller;
using UnityEngine;

namespace FightSystem.Weapon.Melee
{
    public class ThrowingWeapon : MonoBehaviour
    {
        [Header("Attached Compontents")] 
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private LootingItem _lootingItem;

        [Header("Main Params")] [SerializeField]
        private float _lerpSpeed = 2f;
        private int _throwingHp;

        public void Throw(Vector3 direction, float force, int throwingHp)
        {
            _throwingHp = throwingHp;
            if(!_rb) return;
            _rb.AddForce(direction * force, ForceMode.Impulse);
            Rotate();
        }

        private void Rotate()
        {
            if(!_rb) return;
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

private void OnCollisionEnter(Collision other)
        {
            if(!_rb) return;
            MinusItemHp();
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = other.contacts[0].point;
        }
    }
}