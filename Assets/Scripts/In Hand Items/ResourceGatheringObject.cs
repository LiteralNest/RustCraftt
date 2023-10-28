using UnityEngine;

public class ResourceGatheringObject : MonoBehaviour
{
   [Header("Attached")]
   [SerializeField] private Animator _animator;

   [Header("Animator Config")]
   [SerializeField] private string _gatherAnimationTag = "Gather";
   public AnimationClip GatheringAnimation => _gatheringAnimation;
   [SerializeField] private AnimationClip _gatheringAnimation;

   private void OnEnable()
   {
      GlobalEventsContainer.ResourceGatheringObjectAssign?.Invoke(this);
   }

   public void SetGathering(bool value)
   {
      _animator.SetBool(_gatherAnimationTag, value);
   }
}