using UnityEngine;

namespace FightSystem.Weapon.WeaponAnimations
{
    public class ThrowingWeaponAnimator : WeaponAnimator
    {
        private const string AttackingKey = "Attack";

        [ContextMenu("Attack")]
        public void Attack()
            => PlayAnimationServerRpc(AttackingKey);
    }
}