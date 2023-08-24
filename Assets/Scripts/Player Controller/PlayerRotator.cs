using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerRotator : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private PlayerController _playerController;
    [Header("Main Parameters")]
    [SerializeField] private float _rotationSpeed = 3f;

    private Vector2 _touchStartPos;
    private Vector2 _touchEndPos;
    private bool _isTouching;

    private void Start()
    {
        if (_playerController == null)
            _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(Input.touchCount > 0)
            Rotate();
    }
    
    private void Rotate()
    {
        Touch touch = Input.GetTouch(0);
        if (_playerController.IsMoving)
        {
            if(Input.touchCount < 2) return;
            touch = Input.GetTouch(1);
        }
        
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _touchStartPos = touch.position;
                _isTouching = true;
                break;

            case TouchPhase.Moved:
                if (_isTouching)
                {
                    _touchEndPos = touch.position;
                    Vector2 touchDelta = _touchEndPos - _touchStartPos;
                    
                    float rotationX = touchDelta.x * _rotationSpeed * Time.deltaTime;
                    float rotationY = touchDelta.y * _rotationSpeed * Time.deltaTime;

                    transform.Rotate(Vector3.up * rotationX);
                    Camera.main.transform.Rotate(Vector3.left * rotationY);

                    _touchStartPos = _touchEndPos;
                }
                break;

            case TouchPhase.Ended:
                _isTouching = false;
                break;
        }
    }
}
