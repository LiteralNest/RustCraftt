using UnityEngine;
using UnityEngine.EventSystems;

namespace Player_Controller.Looking_Around
{
    public class PlayerRotator : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [Header("Attached Scripts")] [SerializeField]
        private PlayerController _playerController;

        [Header("Main Parameters")] [SerializeField]
        private Vector2 _rotationBounds = new Vector2(0.01f, 0.99f);

        [SerializeField] private Vector2 _rotationBoundsKnockDown = new Vector2(0.25f, -0.25f);
        [SerializeField] private Transform _defaultHead;
        [SerializeField] private Transform _knockDownHead;
        [SerializeField] private float _rotationSpeed = 3f;

        private Transform _rotationTarget;
        private Vector2 _currentRotationBounds;

        private Vector2 _touchStartPos;
        private Vector2 _touchEndPos;
        private bool _isKnockDown;


        private void Awake()
        {
            if (_playerController == null)
                _playerController = FindObjectOfType<PlayerController>();
            SetDefaultHead();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _touchStartPos = eventData.position;
        }

        public void SetDefaultHead()
        {
            _rotationTarget = _defaultHead;
            _currentRotationBounds = _rotationBounds;
            _isKnockDown = false;
        }

        public void SetKnockDownHead()
        {
            _rotationTarget = _knockDownHead;
            _currentRotationBounds = _rotationBoundsKnockDown;
            _isKnockDown = true;
        }

        private void CheckCameraBounds(float rotation)
        {
            var headTransform = _rotationTarget;
            var currentRotation = Mathf.Abs(headTransform.localRotation.x);
            if (rotation < 0)
            {
                if (currentRotation < _currentRotationBounds.x)
                    return;
            }
            else if (currentRotation > _currentRotationBounds.y) return;

            headTransform.Rotate(Vector3.left * rotation, Space.Self);
        }

        private void CheckCameraKnockDownBounds(float rotation)
        {
            var headTransform = _rotationTarget;
            var currentRotation = Mathf.Abs(headTransform.rotation.x);
            if (rotation < 0)
            {
                if (currentRotation < _currentRotationBounds.x)
                    return;
            }
            else if (currentRotation > _currentRotationBounds.y) return;

            headTransform.Rotate(Vector3.left * rotation, Space.Self);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!GlobalValues.CanLookAround) return;

            _touchEndPos = eventData.position;
            Vector2 touchDelta = _touchEndPos - _touchStartPos;

            float rotationX = touchDelta.x * _rotationSpeed * Time.deltaTime;
            float rotationY = touchDelta.y * _rotationSpeed * Time.deltaTime;

            _playerController.transform.Rotate(Vector3.up * rotationX);

            if (!_isKnockDown) CheckCameraBounds(rotationY);
            else CheckCameraKnockDownBounds(rotationY);
            _touchStartPos = _touchEndPos;
        }
    }
}