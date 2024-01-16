using UnityEngine;

namespace InHandItems.InHandAnimations.Weapon
{
    public class ThrowingInHandAnimator : InHandAnimator
    {
        private const string AttackingKey = "Attack";

        [ContextMenu("Attack")]
        public void Attack()
            => PlayAnimationServerRpc(AttackingKey);
    }
}