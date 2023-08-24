using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJumper : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GroundChecker _groundChecker;
    
    [Header("Main Params")]
    [SerializeField] private float _jumpForce = 5f;


    private void Start()
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();
    }
    

    public void Jump()
    {
        if(!_groundChecker.IsGrounded) return;
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}