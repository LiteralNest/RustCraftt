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
    private float _currentMovingSpeed;
    
    [Header("Swim")]
    [SerializeField] private float _swimSpeed;
    public bool IsSwimming { get; set; }
    
    private Rigidbody _rb;

    private float _originalDrag;
    private float _originalAngularDrag;
    
    private readonly float _targetDrag = 0.4f;
    private readonly float _targetAngularDrag = 1f;
    private readonly float _floatStrength = 0.4f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        _originalDrag = _rb.drag;
        _originalAngularDrag = _rb.angularDrag;
        _currentMovingSpeed = _movingSpeed.Value;
    }

    private void FixedUpdate()
    {
        if (IsSwimming != true)
        {
            if (_rb.useGravity != true)
            {
                _rb.useGravity = true;
                _rb.drag = _originalDrag;
                _rb.angularDrag = _originalAngularDrag;
            }
            if (!_ifRunning)
            {
                Move();
                return;
            }
            transform.Translate(Vector3.forward * _currentMovingSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            if (_rb.useGravity) _rb.useGravity = false;
            Swim();
        }
    }

    #region InputMap
    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsSwimming)
        {
            IsMoving = true;
            _move = context.ReadValue<Vector2>();
        }
        else
        {
            IsMoving = true;
            _move = context.ReadValue<Vector2>();
            IsMoving = false;
        }
    }
    

    #endregion
    
    #region Movement
    private void Move()
    {
        Vector3 movement = new Vector3(_move.x, 0f, _move.y);
        if (movement != Vector3.zero)
        {
            _legsAnimator.SetBool("Walking", true);
            _inHandObjectsContainer.SetWalk(true);
            transform.Translate(movement * _currentMovingSpeed * Time.deltaTime, Space.Self);
            return;
        }

        _inHandObjectsContainer.SetWalk(false);
        _legsAnimator.SetBool("Walking", false);
    }

    public void StartRunning()
    {
        _inHandObjectsContainer.SetRun(true);
        _legsAnimator.SetBool("Running", true);
        _ifRunning = true;
        _currentMovingSpeed *= _runningKoef;
    }

    public void StopRunning()
    {
        _inHandObjectsContainer.SetRun(false);
        _legsAnimator.SetBool("Running", false);
        _ifRunning = false;
        _currentMovingSpeed = _movingSpeed.Value;
    }
    

    #endregion

    #region Swimming
    private void Swim()
    {
        Vector3 movement = new Vector3(_move.x, 0f, _move.y);
        if (movement != Vector3.zero)
        {
            _rb.drag = _targetDrag;
            _rb.angularDrag = _targetAngularDrag;
            transform.Translate(movement * _swimSpeed * Time.deltaTime, Space.Self);
            return;
        }
    }

    private void FloatUp()
    {
        if (IsSwimming)
        {
            Vector3 jumpForce = Vector3.up * _floatStrength;
            _rb.AddForce(jumpForce, ForceMode.Force);
        }
    }
    
    

    #endregion
   
}