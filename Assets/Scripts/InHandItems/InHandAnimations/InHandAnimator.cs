using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace InHandItems.InHandAnimations
{
    public abstract class InHandAnimator : NetworkBehaviour 
    {
        [SerializeField] private List<Animator> _fpAnimators;
        [SerializeField] private List<Animator> _tpAnimators;
        
        [ClientRpc]
        private void PlayAnimationClientRpc(string key)
        {
            foreach (var animator in _fpAnimators)
                animator.SetTrigger(key);
            foreach (var animator in _tpAnimators)
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