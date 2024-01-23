namespace InHandItems.InHandAnimations.Weapon
{
    public class BowAnimator : InHandAnimator
    {
        private const string ScopeKey = "Scope";
        private const string AttackKey = "Attack";

        public void SetScope()
            => PlayAnimationServerRpc(ScopeKey);

        public void SetAttack()
            => PlayAnimationServerRpc(AttackKey);
    }
}