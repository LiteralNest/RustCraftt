using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GroundChecker : MonoBehaviour
{
    [field:SerializeField] public bool IsGrounded { get; private set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
            IsGrounded = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
            IsGrounded = false;
    }
}
