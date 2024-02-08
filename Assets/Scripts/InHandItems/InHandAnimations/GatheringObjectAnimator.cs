namespace InHandItems.InHandAnimations
{
    public class GatheringObjectAnimator : InHandAnimator
    {
        private const string AttackKey = "Attack";

        public void Attack(bool value)
            => PlayAnimationServerRpc(AttackKey, value);
    }
}