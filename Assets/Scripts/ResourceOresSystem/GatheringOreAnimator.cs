using System.Collections;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using UnityEngine;

namespace ResourceOresSystem
{
    [RequireComponent(typeof(Animator), typeof(ClientNetworkAnimator))]
    public class GatheringOreAnimator : MonoBehaviour
    {
        [SerializeField] private AnimationClip _fallAnimation;

        private readonly string _fallKey = "Fall";
        private readonly string _idleKey = "Idle";

        private int _fallHash;
        private int _idleHash;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _idleHash = Animator.StringToHash(_idleKey);
            _fallHash = Animator.StringToHash(_fallKey);
        }

        public IEnumerator SetFallRoutine()
        {
            _animator.SetTrigger(_fallHash);
            yield return new WaitForSeconds(_fallAnimation.length);
        }

        public void SetIdle()
        {
            if(_animator)
                _animator.SetTrigger(_idleHash);
        }
    }
}