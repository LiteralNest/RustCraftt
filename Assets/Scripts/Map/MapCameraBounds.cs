using UnityEngine;
using UnityEngine.Serialization;

namespace Map
{
    public class MapCameraBounds : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [Header("Borders")]
        [FormerlySerializedAs("maxX")] [SerializeField] private float _maxX = 450f;
        [FormerlySerializedAs("minX")] [SerializeField] private float _minX = -450f;
        [FormerlySerializedAs("maxY")] [SerializeField] private float _maxY = 400f;
        [FormerlySerializedAs("minY")] [SerializeField] private float _minY = -400f;

        [Header("Zoom")]
        [SerializeField] private float _minZoom = 100f;
        [SerializeField] private float _maxZoom = 420f;

        private Vector2 _initialOffset;
        private float _currentOrthographicSize;

        private void Awake()
        {
            _currentOrthographicSize = _camera.orthographicSize;
       
            // _initialOffset.x = 100f;
            // _initialOffset.y = 100f;
        }

        private void LateUpdate()
        {
            var clampedPosition = _camera.transform.position;

            clampedPosition.x = Mathf.Clamp(clampedPosition.x, _minX, _maxX);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, _minY, _maxY);

            _camera.transform.position = clampedPosition;
        }

        private Vector2 CalculateDynamicOffset()
        {
            var offsetX = _initialOffset.x * (_currentOrthographicSize / _minZoom);
            var offsetY = _initialOffset.y * (_currentOrthographicSize / _minZoom);
            return new Vector2(offsetX, offsetY);
        }

        private void OnDrawGizmos()
        {
            Vector2 dynamicOffset = CalculateDynamicOffset();

            Gizmos.color = Color.red;

            Gizmos.DrawLine(new Vector3(_minX + dynamicOffset.x, 0, _minY + dynamicOffset.y),
                new Vector3(_maxX - dynamicOffset.x, 0, _minY + dynamicOffset.y));
            Gizmos.DrawLine(new Vector3(_minX + dynamicOffset.x, 0, _maxY - dynamicOffset.y),
                new Vector3(_maxX - dynamicOffset.x, 0, _maxY - dynamicOffset.y));

            Gizmos.DrawLine(new Vector3(_minX + dynamicOffset.x, 0, _minY + dynamicOffset.y),
                new Vector3(_minX + dynamicOffset.x, 0, _maxY - dynamicOffset.y));
            Gizmos.DrawLine(new Vector3(_maxX - dynamicOffset.x, 0, _minY + dynamicOffset.y),
                new Vector3(_maxX - dynamicOffset.x, 0, _maxY - dynamicOffset.y));
        }
    }
}
