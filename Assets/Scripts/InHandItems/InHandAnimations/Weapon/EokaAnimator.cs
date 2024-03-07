namespace InHandItems.InHandAnimations.Weapon
{
    public class EokaAnimator : InHandAnimator
    {
        private const string StartFire = "StartFire";
        private const string Fire = "Fire";
        private const string StopFire = "StopFire";
        private const string Reload = "Reload";

        public void PlayStartFire()
            => PlayAnimationServerRpc(StartFire);
        
        public void PlayFire()
            => PlayAnimationServerRpc(Fire);

        public void PlayStopFire()
            => PlayAnimationServerRpc(StopFire);
        
        public void PlayReload()
            => PlayAnimationServerRpc(Reload);
    }
}