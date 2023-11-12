using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GroundChecker : MonoBehaviour
{
    [field:SerializeField] public bool IsGrounded { get; private set; }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Block"))
            IsGrounded = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Block") || other.CompareTag("DamagingItem"))
            IsGrounded = false;
    }
}
