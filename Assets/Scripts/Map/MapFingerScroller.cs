using UnityEngine;

namespace Map
{
    [RequireComponent(typeof(Camera))]
    public class MapFingerScroller : MonoBehaviour
    {
        [SerializeField] private Vector2 _orthographicSizeLimits = new Vector2(75, 350);
        [SerializeField] private float _zoomSpeed = 1f;
        [SerializeField] private float _moveSpeed = 0.5f;
        
        private Vector2 _cachedFingersPosition;

        private Camera _camera;

        private void Start()
            => _camera = GetComponent<Camera>();

        private void Update()
        {
            if (Input.touchCount == 1)
                HandleMovement();
            else if (Input.touchCount == 2)
                HandleScroll();
        }

        private void HandleMovement()
        {
            var touch = Input.GetTouch(0);

            if (touch.phase != TouchPhase.Moved) return;
            var yOffset = touch.deltaPosition.y * _moveSpeed;
            var xOffset = touch.deltaPosition.x * _moveSpeed;

            transform.Translate(xOffset, yOffset, 0f);
        }

        private void HandleScroll()
        {
            var touch = Input.GetTouch(0);
            var touch2 = Input.GetTouch(1);
            var touchDelta = touch.position - touch2.position;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _cachedFingersPosition = touchDelta;
                    break;
                case TouchPhase.Moved:
                    var deltaMagnitudeDiff = _cachedFingersPosition.magnitude - touchDelta.magnitude;
                    var orthographicSize = _camera.orthographicSize;
                    orthographicSize += deltaMagnitudeDiff * _zoomSpeed /* Time.deltaTime*/;

                    _cachedFingersPosition = touchDelta;
                    _camera.orthographicSize = orthographicSize;
                    _camera.orthographicSize =
                        Mathf.Clamp(orthographicSize, _orthographicSizeLimits.x, _orthographicSizeLimits.y);
                    break;
            }
        }
    }
}