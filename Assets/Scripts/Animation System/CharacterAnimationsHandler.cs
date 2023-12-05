using System.Collections.Generic;
using UnityEngine;

namespace Animation_System
{
    public class CharacterAnimationsHandler : MonoBehaviour
    {
        [Header("Animations")] 
        [SerializeField] private string _idleAnimationKey = "Idle";
        [SerializeField] private string _walkAnimationKey = "Walk";
        [SerializeField] private string _swimAnimationKey = "Swim";
        [SerializeField] private string _jumpAnimKey = "Jump";
        [SerializeField] private string _knockDownAnim = "KnockDown";
        [SerializeField] private string _fallAnim = "Fall";
        [SerializeField] private string _deathAnim = "Death";

        [Header("Animators")] 
        [SerializeField] private List<Animator> _animators = new List<Animator>();

        private void SetKey(string key, Animator anim)
        {
            anim.SetBool(_idleAnimationKey, _idleAnimationKey == key);
            anim.SetBool(_walkAnimationKey, _walkAnimationKey == key);
            anim.SetBool(_swimAnimationKey, _swimAnimationKey == key);
            anim.SetBool(_jumpAnimKey, _jumpAnimKey == key);
            anim.SetBool(_knockDownAnim, _knockDownAnim == key);
            anim.SetBool(_fallAnim, _fallAnim == key);
            anim.SetBool(_deathAnim, _deathAnim == key); 
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

        [ContextMenu("Jump")]
        public void SetJump()
            => SetAnimation(_jumpAnimKey);
        
        [ContextMenu("Knock Down")]
        public void SetKnockDown()
            => SetAnimation(_knockDownAnim);
        
        [ContextMenu("Fall")]
        public void SetFall()
            => SetAnimation(_fallAnim);
        
        [ContextMenu("Death")]
        public void SetDeath()
            => SetAnimation(_deathAnim);
    }
}
