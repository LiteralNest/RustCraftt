namespace InHandItems.InHandAnimations
{
    public class InHandHealAnimator : InHandAnimator
    {
        private const string HealKey = "Heal";

        public void PlayHeal()
            => PlayAnimationServerRpc(HealKey);
    }
}