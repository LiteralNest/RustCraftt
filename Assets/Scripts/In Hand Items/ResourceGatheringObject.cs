using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ResourceGatheringObject : MonoBehaviour
{
   [Header("Attached")]
   [SerializeField] private Animator _animator;

   [Header("Animator Config")]
   [SerializeField] private string _gatherAnimationTag = "Gather";  
   private void Start()
   {
      if(_animator == null)
         _animator = GetComponent<Animator>();
   }
 
   private void OnEnable()
   {
      GlobalEventsContainer.ResourceGatheringObjectAssign?.Invoke(this);
   }

   public void SetGathering(bool value)
   {
      _animator.SetBool(_gatherAnimationTag, value);
   }
}