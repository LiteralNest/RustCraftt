using UnityEngine;

namespace Vehicle.Animators
{
    [RequireComponent(typeof(Animator))]
    public class HorseAnimator : MonoBehaviour
    {
        private readonly string _walkKey = "Walk";

        private int _walkHash;

        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _walkHash = Animator.StringToHash(_walkKey);
        }

        public void HandleWalk(bool value)
            => _animator.SetBool(_walkHash, value);
    }
}