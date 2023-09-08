using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPlacer : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private GameObject _targetPanel;

    [Header("Start Init")] [SerializeField]
    private LayerMask _snapLayer;

    [SerializeField] private float _distance = 5f;

    [Header("In game init")] [SerializeField]
    private PlacingObject _target;

    private Camera _targetCamera;

    private void Start()
        => _targetCamera = Camera.main;

    private void FixedUpdate()
    {
        if (_target == null) return;
        TryRayCast();
    }

    public void SetObject(PlacingObject target)
    {
        if (target == null)
        {
            _target = null;
            _targetPanel.SetActive(false);
            return;
        }

        if (_target != null)
            Destroy(_target.gameObject);
        var instance = Instantiate(target);
        _target = instance;
        _targetPanel.SetActive(true);
    }

    private Vector3 GetFrontOfCameraPosition()
        => _targetCamera.transform.position + _targetCamera.transform.forward * _distance;

    private void TryRayCast()
    {
        Vector3 rayOrigin = _targetCamera.transform.position;
        Vector3 rayDirection = _targetCamera.transform.forward;
        RaycastHit hit;

        var targetTransform = _target.transform;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _snapLayer))
        {
            if (hit.transform.CompareTag(_target.RequiredTag))
            {
                if (!hit.transform.TryGetComponent<Snap>(out var snap)) return;
                targetTransform.position = snap.transform.position;
                targetTransform.rotation = snap.transform.rotation;
                _target.CanBePlaced = true;
                return;
            }
        }

        _target.CanBePlaced = false;
        targetTransform.position = GetFrontOfCameraPosition();
    }

    public void Place()
    {
        if (_target == null || !_target.CanBePlaced) return;
        SetObject(null);
    }

    public void ClearCurrentPref()
    {
        if (_target != null)
            Destroy(_target.gameObject);
        SetObject(null);
    }
}