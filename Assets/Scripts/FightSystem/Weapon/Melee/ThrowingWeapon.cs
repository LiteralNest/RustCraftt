using System.Collections;
using FightSystem.Weapon.ThrowingWeapon;
using Items_System.Items;
using UnityEngine;

namespace FightSystem.Weapon.Melee
{
    public class ThrowingWeapon : BaseThrowingWeapon
    {
        [Header("Attached Components")] [SerializeField]
        private BoxCollider _collider;
        
        private int _throwingHp;
        private Collision _hitObject;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(WaitForEnable());
        }

        private IEnumerator WaitForEnable()
        {
            _collider.enabled = false;
            yield return new WaitForSeconds(0.05f);
            _collider.enabled = true;
        }

        public override void Throw(float angle, int throwingHp = -1)
        {
            base.Throw(angle, throwingHp);
            _throwingHp = throwingHp;
            TargetLootingItem.InitByTargetItem(throwingHp);
        }

        private void MinusItemHp()
        {
            var item = TargetLootingItem.TargetItem as DamagableItem;
            var minusingHp = item.Hp / 10;
            var currentHp = _throwingHp - minusingHp;
            TargetLootingItem.InitByTargetItem(currentHp, currentHp <= 0);
        }

        protected override void OnCollisionEnter(Collision other)
        {
            MinusItemHp();
            base.OnCollisionEnter(other);
        }
    }
}