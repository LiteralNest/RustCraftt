using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    [Header("Move")]
    [SerializeField] private NetworkVariable<float> _movingSpeed = new(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public bool IsMoving { get; private set; }
    private Vector2 _move;

    [Header("Run")] 
    [SerializeField] 
    private float _runningKoef = 1.5f;
    private bool _ifRunning;

    private float _currentMovingCpeed;
    
    private void Start()
    => _currentMovingCpeed = _movingSpeed.Value;

    private void Update()
    {
        if (!_ifRunning)
        {
            _animator.SetBool("Running", false);
            Move();
            return;
        }
        _animator.SetBool("Running", true);
        transform.Translate(Vector3.forward * _currentMovingCpeed * Time.deltaTime, Space.Self);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        IsMoving = true;
        _move = context.ReadValue<Vector2>();
        IsMoving = false;
    }

    private void Move()
    {
        Vector3 movement = new Vector3(_move.x, 0f, _move.y);
        if (movement != Vector3.zero)
        {
            _animator.SetBool("Walking", true);
            transform.Translate(movement * _currentMovingCpeed * Time.deltaTime, Space.Self);
            return;
        }
        _animator.SetBool("Walking", false);
    }

    public void StartRunning()
    {
        _ifRunning = true;
        _currentMovingCpeed *= _runningKoef;
    }

    public void StopRunning()
    {
        _ifRunning = false;
        _currentMovingCpeed = _movingSpeed.Value;
    }

    [ContextMenu("Change variable value")]
    private void ChangeMovingSpeedVar()
    {
        _movingSpeed.Value = 10;
    }
}