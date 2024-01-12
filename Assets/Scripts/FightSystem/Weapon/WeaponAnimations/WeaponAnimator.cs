using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Weapon.WeaponAnimations
{
    public abstract class WeaponAnimator : NetworkBehaviour 
    {
        [SerializeField] private List<Animator> _animators;
        
        [ClientRpc]
        private void PlayAnimationClientRpc(string key)
        {
            foreach (var animator in _animators)
                animator.SetTrigger(key);
        }

        [ServerRpc(RequireOwnership = false)]
        protected void PlayAnimationServerRpc(string key)
        {
            if (!IsServer) return;
            PlayAnimationClientRpc(key);
        }
    }
}