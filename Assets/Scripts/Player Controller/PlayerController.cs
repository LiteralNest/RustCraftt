using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float _movingSpeed = 5;

    public bool IsMoving { get; private set; }
    private Vector2 _move;

    [Header("Run")] 
    [SerializeField] 
    private float _runningKoef = 1.5f;
    private bool _ifRunning;
    
    private void Update()
    {
        if (!_ifRunning)
        {
            Move();
            return;
        }
        transform.Translate(Vector3.forward * _movingSpeed * Time.deltaTime, Space.Self);
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
        
        transform.Translate(movement * _movingSpeed * Time.deltaTime, Space.Self);
    }

    public void StartRunning()
    {
        _ifRunning = true;
        _movingSpeed *= _runningKoef;
    }

    public void StopRunning()
    {
        _ifRunning = false;
        _movingSpeed /= _runningKoef;
    }
}