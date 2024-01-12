namespace FightSystem.Weapon.WeaponAnimations
{
    public class RifleAnimator : WeaponAnimator
    {
        private const string Reload = "Reload";
        private const string Shot = "Shot"; 
        
        public void PlayReload()
            => PlayAnimationServerRpc(Reload);
        
        public void PlayShot()
            => PlayAnimationServerRpc(Shot);
    }
}