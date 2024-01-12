using UnityEngine;

namespace FightSystem.Weapon.WeaponAnimations
{
    public class EokaAnimator : WeaponAnimator
    {
        private const string StartFire = "StartFire";
        private const string MissFire = "MissFire";
        private const string Fire = "Fire";
        
        [ContextMenu("PlayStartFire")]
        public void PlayStartFire()
            => PlayAnimationServerRpc(StartFire);
    }
}