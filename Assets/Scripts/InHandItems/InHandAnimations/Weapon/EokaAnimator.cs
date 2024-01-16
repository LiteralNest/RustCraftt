using UnityEngine;

namespace InHandItems.InHandAnimations.Weapon
{
    public class EokaAnimator : InHandAnimator
    {
        private const string StartFire = "StartFire";
        private const string MissFire = "MissFire";
        private const string Fire = "Fire";
        
        [ContextMenu("PlayStartFire")]
        public void PlayStartFire()
            => PlayAnimationServerRpc(StartFire);
    }
}