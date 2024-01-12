using UnityEngine;

namespace InHandItems.InHand
{
    public class InHandDefaultHands : InHandObject
    {
        [Header("Attached Scripts")]
        [SerializeField] private Animator _animator;
        
        [Header("Animator States")]
        [SerializeField] private string _attackIndex = "Attacking";
        [SerializeField] private string _walkIndex = "Walking";
        [SerializeField] private string _runIndex = "Running";
        
        public override void Walk(bool value)
        {
            if(!_animator) return;
            _animator.SetBool(_runIndex, false);
            _animator.SetBool(_walkIndex, value);
        }
        
        public override void Run(bool value)
        {
            if(!_animator) return;
            _animator.SetBool(_runIndex, value);
            _animator.SetBool(_walkIndex, false);
        }

        public override void HandleAttacking(bool attack)
        {
            if(!_animator) return;
            _animator.SetBool(_attackIndex, attack);
        }
    }
}