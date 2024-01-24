namespace InHandItems.InHandAnimations.Weapon
{
    public class EokaAnimator : InHandAnimator
    {
        private const string StartFire = "StartFire";
        private const string MissFire = "MissFire";
        private const string Fire = "Fire";
        private const string StopFire = "StopFire";

        public void PlayStartFire()
            => PlayAnimationServerRpc(StartFire);

        public void PlayMissFire()
            => PlayAnimationServerRpc(MissFire);

        public void PlayFire()
            => PlayAnimationServerRpc(Fire);

        public void PlayStopFire()
            => PlayAnimationServerRpc(StopFire);
    }
}