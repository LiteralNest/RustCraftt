namespace InHandItems.InHandAnimations.Weapon
{
    public class BowAnimator : InHandAnimator
    {
        private const string ScopeKey = "Scope";
        private const string AttackKey = "Attack";
        private const string IdleKey = "Idle";

        public void SetScope()
            => PlayAnimationServerRpc(ScopeKey);

        public void SetAttack()
            => PlayAnimationServerRpc(AttackKey);
        
        public void SetIdle()
            => PlayAnimationServerRpc(IdleKey);
    }
}