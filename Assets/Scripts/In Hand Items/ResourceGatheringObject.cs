using UI;
using UnityEngine;

public class ResourceGatheringObject : MonoBehaviour
{
   [Header("Attached")]
   [SerializeField] private Animator _animator;

   [Header("Animator Config")]
   [SerializeField] private string _gatherAnimationTag = "Gather";
   public AnimationClip GatheringAnimation => _gatheringAnimation;
   [SerializeField] private AnimationClip _gatheringAnimation;
   [SerializeField] private bool _canAttack = true;
   private void OnEnable()
   {
      if(_canAttack)
         CharacterUIHandler.singleton.ActivateGatherButton(true);
      GlobalEventsContainer.ResourceGatheringObjectAssign?.Invoke(this);
   }
   
   private void OnDisable()
   {
      if(_canAttack)
         CharacterUIHandler.singleton.ActivateGatherButton(false);
   }

   public void SetGathering(bool value)
   {
      _animator.SetBool(_gatherAnimationTag, value);
   }
}