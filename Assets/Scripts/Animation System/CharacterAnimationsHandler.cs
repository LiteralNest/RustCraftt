using System.Collections.Generic;
using UnityEngine;

namespace Animation_System
{
    public class CharacterAnimationsHandler : MonoBehaviour
    {
        [Header("Animations")] 
        [SerializeField] private string _idleAnimationKey;
        [SerializeField] private string _walkAnimationKey;
        [SerializeField] private string _swimAnimationKey;

        [Header("Animators")] 
        [SerializeField] private List<Animator> _animators = new List<Animator>();

        private void SetKey(string key, Animator anim)
        {
            anim.SetBool(_idleAnimationKey, _idleAnimationKey == key);
            anim.SetBool(_walkAnimationKey, _walkAnimationKey == key);
            anim.SetBool(_swimAnimationKey, _swimAnimationKey == key);
        }
        
        private void SetAnimation(string animationKey)
        {
            foreach (var anim in _animators)
                SetKey(animationKey, anim);

        }
        
        [ContextMenu("Set Idle")]
        public void SetIdle()
            => SetAnimation(_idleAnimationKey);
        
        [ContextMenu("Set Walk")]
        public void SetWalk()
            => SetAnimation(_walkAnimationKey);
        
        [ContextMenu("Set Swim")]
        public void SetSwim()
            => SetAnimation(_swimAnimationKey);
    }
}
