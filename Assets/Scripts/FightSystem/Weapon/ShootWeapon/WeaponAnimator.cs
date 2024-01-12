using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon
{
    public class WeaponAnimator : NetworkBehaviour
    {
        private const string Reload = "Reload";
        private const string Shot = "Shot";
        
        [SerializeField] private List<Animator> _animators;
        
        public void PlayReload()
            => PlayAnimationServerRpc(Reload);
        
        public void PlayShot()
            => PlayAnimationServerRpc(Shot);

        [ClientRpc]
        private void PlayAnimationClientRpc(string key)
        {
            foreach (var animator in _animators)
                animator.SetTrigger(key);
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayAnimationServerRpc(string key)
        {
            if (!IsServer) return;
            PlayAnimationClientRpc(key);
        }
    }
}