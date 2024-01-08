using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    public class WeaponAnimator : NetworkBehaviour
    {
        [SerializeField] private List<Animator> _animators;

        [ContextMenu("Reload")]
        public void PlayReload()
            => PlayAnimationServerRpc("Reload");

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