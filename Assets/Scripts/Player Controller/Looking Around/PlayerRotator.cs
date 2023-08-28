using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRotator : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [Header("Attached Scripts")]
    [SerializeField] private PlayerController _playerController;
    [Header("Main Parameters")]
    [SerializeField] private float _rotationSpeed = 3f;
    
    private Vector2 _touchStartPos;
    private Vector2 _touchEndPos;

    private void Awake()
    {
        if (_playerController == null)
            _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        // if(Input.touchCount > 0)
        //     Rotate();
    }
    
    // private void Rotate()
    // {
    //     Touch touch = Input.GetTouch(0);
    //     if (_playerController.IsMoving)
    //     {
    //         if(Input.touchCount < 2) return;
    //         touch = Input.GetTouch(1);
    //     }
    //     
    //     switch (touch.phase)
    //     {
    //         case TouchPhase.Began:
    //             _touchStartPos = touch.position;
    //             break;
    //
    //         case TouchPhase.Moved:
    //             if (_isTouching)
    //             {
    //                 _touchEndPos = touch.position;
    //                 Vector2 touchDelta = _touchEndPos - _touchStartPos;
    //                 
    //                 float rotationX = touchDelta.x * _rotationSpeed * Time.deltaTime;
    //                 float rotationY = touchDelta.y * _rotationSpeed * Time.deltaTime;
    //
    //                 transform.Rotate(Vector3.up * rotationX);
    //                 Camera.main.transform.Rotate(Vector3.left * rotationY);
    //
    //                 _touchStartPos = _touchEndPos;
    //             }
    //             break;
    //
    //         case TouchPhase.Ended:
    //             _isTouching = false;
    //             break;
    //     }
    // }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _touchStartPos = eventData.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _touchEndPos = eventData.position;
        Vector2 touchDelta = _touchEndPos - _touchStartPos;
        Debug.Log(touchDelta.x + " " + touchDelta.y);       
        float rotationX = touchDelta.x * _rotationSpeed * Time.deltaTime;
        float rotationY = touchDelta.y * _rotationSpeed * Time.deltaTime;

        _playerController.transform.Rotate(Vector3.up * rotationX);
        Camera.main.transform.Rotate(Vector3.left * rotationY);

        _touchStartPos = _touchEndPos;
    }
}
