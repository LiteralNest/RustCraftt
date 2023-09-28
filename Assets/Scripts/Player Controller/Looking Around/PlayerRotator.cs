using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRotator : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [Header("Attached Scripts")] [SerializeField]
    private PlayerHandsController _playerHandsController;
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private Transform _head;
    [Header("Main Parameters")]
    [SerializeField] private float _rotationSpeed = 3f;
    private Vector2 _touchStartPos;
    private Vector2 _touchEndPos;

    private void Awake()
    {
        if (_playerController == null)
            _playerController = FindObjectOfType<PlayerController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchStartPos = eventData.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if(!GlobalValues.CanLookAround) return;
        _touchEndPos = eventData.position;
        // _playerHandsController.MoveHands();
        Vector2 touchDelta = _touchEndPos - _touchStartPos;

        float rotationX = touchDelta.x * _rotationSpeed * Time.deltaTime;
        float rotationY = touchDelta.y * _rotationSpeed * Time.deltaTime;

        _playerController.transform.Rotate(Vector3.up * rotationX);
        _head.transform.Rotate(Vector3.left * rotationY);
        _touchStartPos = _touchEndPos;
    }
}
