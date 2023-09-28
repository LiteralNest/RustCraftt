using UnityEngine;

public class PlayerObjectsPlacer : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;
    [SerializeField] private float _distance = 5f;

    private PlacingObjectBluePrint _targetBluePrint;

    private void Update()
    {
        if (_targetBluePrint == null) return;
        TryMoveBuildingObject();
    }

    private Vector3 GetFrontOfCameraPosition()
        => _targetCamera.transform.position + _targetCamera.transform.forward * _distance;

    private void TryMoveBuildingObject()
    {
        if (!_targetBluePrint.TryGetObjectCoords(_targetCamera, out var coords))
        {
            _targetBluePrint.SetCanBePlaced(false);
            _targetBluePrint.transform.position = GetFrontOfCameraPosition();
            return;
        }

        _targetBluePrint.SetCanBePlaced(true);
        _targetBluePrint.transform.position = coords;
    }

    public void Place()
    {
        if (_targetBluePrint == null) return;
        _targetBluePrint.TryPlace();
        GlobalEventsContainer.ShouldDisplayPlacingPanel?.Invoke(false);
        ClearCurrentPref();
    }

    public void ClearCurrentPref()
    {
        if (_targetBluePrint == null) return;
        Destroy(_targetBluePrint.gameObject);
        GlobalEventsContainer.ShouldDisplayBuildingStaff?.Invoke(false);
        _targetBluePrint = null;
    }

    public void SetCurrentPref(PlacingObjectBluePrint target)
    {
        ClearCurrentPref();
        _targetBluePrint = Instantiate(target);
    }

    public void Rotate()
    {
        if (_targetBluePrint == null) return;
        _targetBluePrint.Rotate();
    }

    public void Reset()
    {
        ClearCurrentPref();
        GlobalEventsContainer.ShouldDisplayPlacingPanel?.Invoke(false);
    }
}