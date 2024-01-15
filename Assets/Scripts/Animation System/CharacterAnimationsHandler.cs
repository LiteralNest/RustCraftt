using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Animation_System
{
    public class CharacterAnimationsHandler : NetworkBehaviour
    {
        [Header("Animations")] [SerializeField]
        private List<CharacterAnimationSlot> _keys = new List<CharacterAnimationSlot>();

        [Header("Animators")] [SerializeField] private List<Animator> _animators = new List<Animator>();
        
        public int GetAnimationNum(string key)
        {
            for (int i = 0; i < _keys.Count; i++)
            {
                if (_keys[i].Key != key) continue;
                return i;
            }

            Debug.LogError("Can't find animation: " + key);
            return 0;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetAnimationServerRpc(int num)
        {
            if(!IsServer) return;
            SetAnimationClientRpc(num);
        }

        [ClientRpc]
        private void SetAnimationClientRpc(int num)
        {
            SetAnimation(num);
        }
        
        public void SetAnimation(int num)
        {
            var changingSlot = _keys[num];
            foreach (var anim in _animators)
            {
                foreach (var slot in _keys)
                {
                    if (slot.Key == changingSlot.Key || slot.Type == AnimationType.Trigger) continue;
                    anim.SetBool(slot.Key, false);
                }

                if (changingSlot.Type == AnimationType.Bool)
                    anim.SetBool(changingSlot.Key, true);
                else if (changingSlot.Type == AnimationType.Trigger)
                    anim.SetTrigger(changingSlot.Key);
            }
        }
    }
}