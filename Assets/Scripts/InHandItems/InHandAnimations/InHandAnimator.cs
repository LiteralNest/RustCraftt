using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace InHandItems.InHandAnimations
{
    public abstract class InHandAnimator : NetworkBehaviour 
    {
        [SerializeField] private List<Animator> _fpAnimators;
        [SerializeField] private List<Animator> _tpAnimators;
        
        

        [ServerRpc(RequireOwnership = false)]
        protected void PlayAnimationServerRpc(string key)
        {
            if (!IsServer) return;
            PlayAnimationClientRpc(key);
        }

        [ServerRpc]
        protected void PlayAnimationServerRpc(string key, bool value)
        {
            if (!IsServer) return;
            PlayAnimationClientRpc(key, value);
        }
        
        [ClientRpc]
        private void PlayAnimationClientRpc(string key)
        {
            foreach (var animator in _fpAnimators)
                animator.SetTrigger(key);
            foreach (var animator in _tpAnimators)
                animator.SetTrigger(key);
        }
        
        [ClientRpc]
        private void PlayAnimationClientRpc(string key, bool value)
        {
            foreach (var animator in _fpAnimators)
                animator.SetBool(key, value);
            foreach (var animator in _tpAnimators)
                animator.SetBool(key, value);
        }
    }
}