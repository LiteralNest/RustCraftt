using UnityEngine;

namespace InHandItems.InHandAnimations.Weapon
{
    public class ThrowingInHandAnimator : InHandAnimator
    {
        private const string AttackingKey = "Attack";
        private const string ThrowKey = "Throw";
        private const string ScopeKey = "Scope";
        
        public void HandleAttack(bool value)
            => PlayAnimationServerRpc(AttackingKey, value);
        
        public void SetThrow()
            => PlayAnimationServerRpc(ThrowKey);
        
        public void SetScope()
            => PlayAnimationServerRpc(ScopeKey);
    }
}