namespace InHandItems.InHandAnimations.Weapon
{
    public class ExplosiveAnimator : InHandAnimator
    {
        private const string ThrowKey = "Throw";
        
        public void SetThrow()
            => PlayAnimationServerRpc(ThrowKey);
    }
}