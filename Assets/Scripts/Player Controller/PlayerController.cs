using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private InHandObjectsContainer _inHandObjectsContainer;
    
    [Header("Animators")]
    [SerializeField] private Animator _handsAnimator;
    [SerializeField] private Animator _legsAnimator;
    
    [Header("Move")]
    [SerializeField] private NetworkVariable<float> _movingSpeed = new(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public bool IsMoving { get; private set; }
    private Vector2 _move;

    [Header("Run")] 
    [SerializeField] 
    private float _runningKoef = 1.5f;
    private bool _ifRunning;

    private float _currentMovingCpeed;
    private Rigidbody _rb;

    [Header("Swim")]
    [SerializeField] private float _swimSpeed;
    [SerializeField] private Transform _target;
    public bool IsSwimming { get; set; }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _currentMovingCpeed = _movingSpeed.Value;
    }

    private void FixedUpdate()
    {
        if (IsSwimming != true)
        {
            if (_rb.useGravity != true)
                _rb.useGravity = true;
            if (!_ifRunning)
            {
                Move();
                return;
            }
            transform.Translate(Vector3.forward * _currentMovingCpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            if (_rb.useGravity) 
                _rb.useGravity = false;

            if (_move.y > 0) transform.position += _target.forward * _swimSpeed * Time.deltaTime;
             if (_move.y < 0) transform.position -= _target.forward * _swimSpeed * Time.deltaTime;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsSwimming)
        {
            _move = context.ReadValue<Vector2>();
        }
        else
        {
            IsMoving = true;
            _move = context.ReadValue<Vector2>();
            IsMoving = false;
        }
      
    }

    private void Move()
    {
        Vector3 movement = new Vector3(_move.x, 0f, _move.y);
        if (movement != Vector3.zero)
        {
            _legsAnimator.SetBool("Walking", true);
            _inHandObjectsContainer.SetWalk(true);
            transform.Translate(movement * _currentMovingCpeed * Time.deltaTime, Space.Self);
            return;
        }

        _inHandObjectsContainer.SetWalk(false);
        _legsAnimator.SetBool("Walking", false);
    }

    private void Swim()
    {
        Vector3 movement = new Vector3(_move.x, 0f, _move.y);
        if (movement != Vector3.zero)
        {
            // Add swimming logic here
            transform.Translate(movement * _swimSpeed * Time.deltaTime, Space.Self);
            return;
        }
    }
    
    public void StartRunning()
    {
        _inHandObjectsContainer.SetRun(true);
        _legsAnimator.SetBool("Running", true);
        _ifRunning = true;
        _currentMovingCpeed *= _runningKoef;
    }

    public void StopRunning()
    {
        _inHandObjectsContainer.SetRun(false);
        _legsAnimator.SetBool("Running", false);
        _ifRunning = false;
        _currentMovingCpeed = _movingSpeed.Value;
    }
}