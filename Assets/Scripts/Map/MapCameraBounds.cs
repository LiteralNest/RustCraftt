using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] private float _zoomSpeed = 4f;
    
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
        var dynamicOffset = CalculateDynamicOffset();
        var clampedPosition = _camera.transform.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, _minX + dynamicOffset.x, _maxX - dynamicOffset.x);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, _minY + dynamicOffset.y, _maxY - dynamicOffset.y);

        _camera.transform.position = clampedPosition;
    }

    private void OnGUI()
    {
        var buttonWidth = 100f;
        var buttonHeight = 100f;
    
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;
    
        var buttonX = (screenWidth - buttonWidth) / 2f;
        var buttonY = 10f;
    
        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Zoom In"))
        {
            ZoomCamera(-10);
        }
    
        buttonY += buttonHeight + 10f;
    
        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Zoom Out"))
        {
            ZoomCamera(10);
        }
    }

    private void ZoomCamera(int direction)
    {
        
        
        _currentOrthographicSize += direction * _zoomSpeed;
        _currentOrthographicSize = Mathf.Clamp(_currentOrthographicSize, _minZoom, _maxZoom);
        
        _camera.orthographicSize = _currentOrthographicSize;
        
        _initialOffset = CalculateInitialOffset();
    }

    private Vector2 CalculateInitialOffset()
    {
        var offsetX = _initialOffset.x * (_currentOrthographicSize / _minZoom);
        var offsetY = _initialOffset.y * (_currentOrthographicSize / _minZoom);
        return new Vector2(offsetX, offsetY);
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
