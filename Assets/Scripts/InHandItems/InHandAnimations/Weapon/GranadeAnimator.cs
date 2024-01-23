namespace InHandItems.InHandAnimations.Weapon
{
    public class GranadeAnimator : InHandAnimator
    {
        private const string ScopeKey = "Scope";
        private const string ThrowKey = "Throw";

        public void PlayScope()
            => PlayAnimationServerRpc(ScopeKey);

        public void PlayThrow()
            => PlayAnimationServerRpc(ThrowKey);
    }
}