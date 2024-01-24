using UnityEngine;

namespace AI.Animals.Animators
{
    [RequireComponent(typeof(Animator))]
    public class AnimalAnimator : MonoBehaviour
    {
        private Animator _animator;

        private readonly string _attackKey = "Attack";
        private readonly string _walkKey = "Walk";
        private readonly string _runKey = "Run";
        private readonly string _idleKey = "Idle";

        private int _attackHash;
        private int _walkHash;
        private int _runHash;
        private int _idleHash;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _attackHash = Animator.StringToHash(_attackKey);
            _walkHash = Animator.StringToHash(_walkKey);
            _runHash = Animator.StringToHash(_runKey);
            _idleHash = Animator.StringToHash(_idleKey);
        }
        
        public void SetAttack()
            => _animator.SetTrigger(_attackHash);
        
        public void SetWalk()
            => _animator.SetTrigger(_walkHash);
        
        public void SetRun()
            => _animator.SetTrigger(_runHash);
        
        public void SetIdle()
            => _animator.SetTrigger(_idleHash);
    }
}