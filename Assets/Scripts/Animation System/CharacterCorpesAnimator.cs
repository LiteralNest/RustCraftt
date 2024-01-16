using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Animation_System
{
    public class CharacterCorpesAnimator : NetworkBehaviour
    {
        private const string DeathAnimKey = "Dead";

        [SerializeField] private List<Animator> _animators;

        [ServerRpc(RequireOwnership = false)]
        public void DisplayDeathServerRpc()
        {
            if (!IsServer) return;
            DisplayDeathClientRpc();
        }

        private void DisplayDeath()
        {
            foreach (var anim in _animators)
                anim.SetTrigger(Animator.StringToHash(DeathAnimKey));
        }

        [ClientRpc]
        private void DisplayDeathClientRpc()
        {
            DisplayDeath();
        }
    }
}