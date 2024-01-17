using UnityEngine;
using UnityEngine.EventSystems;

namespace Player_Controller.Looking_Around
{
    public class PlayerRotator : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [Header("Attached Scripts")] [SerializeField]
        private PlayerController _playerController;

        [Header("Main Parameters")] [SerializeField]
        private Vector2 _rotationBounds = new Vector2(0.75f, -0.25f);

        [SerializeField] private Vector2 _rotationBoundsKnockDown = new Vector2(0.25f, -0.25f);
        [SerializeField] private Transform _defaultHead;
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

        private void CheckCameraBounds(float rotation)
        {
            var headTransform = _defaultHead;
            var currentRotation = Mathf.Abs(headTransform.localRotation.x);
            if (rotation < 0)
            {
                if (currentRotation < _rotationBounds.x)
                    return;
            }
            else if (currentRotation > _rotationBounds.y) return;

            headTransform.Rotate(Vector3.left * rotation, Space.Self);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!GlobalValues.CanLookAround) return;

            _touchEndPos = eventData.position;
            Vector2 touchDelta = _touchEndPos - _touchStartPos;

            float rotationX = touchDelta.x * _rotationSpeed * Time.deltaTime;
            float rotationY = touchDelta.y * _rotationSpeed * Time.deltaTime;
            // rotationY = Mathf.Clamp(rotationY, -90f, 90f);

            _playerController.transform.Rotate(Vector3.up * rotationX);

            CheckCameraBounds(rotationY);
            _touchStartPos = _touchEndPos;
        }
    }
}