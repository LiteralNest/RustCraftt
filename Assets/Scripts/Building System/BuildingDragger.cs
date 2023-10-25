using UnityEngine;

public class BuildingDragger : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;
    [SerializeField] private BluePrint _currentPref;
    [SerializeField] private float _distance = 5f;

    private void Update()
    {
        if (_currentPref == null) return;
        TryMoveBuildingObject();
    }

    private Vector3 GetFrontOfCameraPosition()
        => _targetCamera.transform.position + _targetCamera.transform.forward * _distance;

    private void TryMoveBuildingObject()
    {
        if (!_currentPref.TryGetObjectCoords(_targetCamera, out var coords))
        {
            _currentPref.SetOnFrontOfPlayer(true);
            _currentPref.transform.position = GetFrontOfCameraPosition();
            return;
        }
        _currentPref.SetOnFrontOfPlayer(false);
        _currentPref.transform.position = coords;
    }

    public void Place()
    {
        if (_currentPref == null) return;
        _currentPref.Place();
    }

    public void ClearCurrentPref()
    {
        if(_currentPref == null) return;
        Destroy(_currentPref.gameObject);
        GlobalEventsContainer.ShouldDisplayBuildingStaff?.Invoke(false);
        _currentPref = null;
    }
    
    public void SetCurrentPref(BluePrint target)
    {
        ClearCurrentPref();
        _currentPref = target;
    }

    public void Rotate()
    {
        if(_currentPref == null) return;
        _currentPref.Rotate();
    }
}