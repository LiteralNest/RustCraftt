using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPlacer : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private GameObject _targetPanel;

    [Header("In game init")] [SerializeField]
    private ObjectBluePrint _target;
    [SerializeField] private Camera _targetCamera;

    private void Start()
        => _targetCamera = Camera.main;

    private void FixedUpdate()
    {
        if (_target == null) return;
        _target.TryGetObjectCoords(_targetCamera, out var coords);
    }

    public void SetObject(ObjectBluePrint target)
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


    public void Place()
    {
        if (_target == null || !_target.CanBePlaced) return;
        if(!_target.TryPlaceObject()) return;
        SetObject(null);
    }

    public void ClearCurrentPref()
    {
        if (_target != null)
            Destroy(_target.gameObject);
        SetObject(null);
    }
}