namespace InHandItems.InHandAnimations.Weapon
{
    public class RifleAnimator : InHandAnimator
    {
        private const string Reload = "Reload";
        private const string Shot = "Shot"; 
        
        public void PlayReload()
            => PlayAnimationServerRpc(Reload);
        
        public void PlayShot()
            => PlayAnimationServerRpc(Shot);
    }
}