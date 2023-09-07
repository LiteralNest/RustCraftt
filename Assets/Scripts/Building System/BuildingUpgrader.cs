using UnityEngine;
using UnityEngine.Serialization;

public class BuildingUpgrader : MonoBehaviour
{
    [Header("Main Params")]
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private GameObject _upgradeDisplaying;
    [SerializeField] private Camera _targetCamera;
    [SerializeField] private bool _canUpgrade;

    [Header("UI")] [SerializeField] private GameObject _hammerPanel;
    
    private BuildingObject _targetBuildingObject;
    
    private void OnEnable()
        => GlobalEventsContainer.BuildingHammerActivated += OnBuildingHammerActivated;
    
    private void OnDisable()
        => GlobalEventsContainer.BuildingHammerActivated -= OnBuildingHammerActivated;

    private void Start()
    {
        if(_targetCamera == null)
            _targetCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if(!_canUpgrade) return;
        _upgradeDisplaying.gameObject.SetActive(false);
        _targetBuildingObject = GetTargetBuildingObject();
        if(!CanUpgradeObject()) return;
        _upgradeDisplaying.SetActive(true);
    }

    private void OnBuildingHammerActivated(bool value)
        => _canUpgrade = value;
    
    private bool CanUpgradeObject()
        => !(_targetBuildingObject == null || !_targetBuildingObject.CanBeUpgrade());
    
    private BuildingObject GetTargetBuildingObject()
    {
        Vector3 rayOrigin = _targetCamera.transform.position;
        Vector3 rayDirection = _targetCamera.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _targetMask))
        {
            if (hit.transform.CompareTag("Building"))
            {
                if (!hit.transform.TryGetComponent<BuildingObject>(out BuildingObject res)) return null;
                return res;
            }
        }
        return null;
    }

    public void Upgrade()
    {
        if(!CanUpgradeObject()) return;
        _targetBuildingObject.TryUpgrade();
        _hammerPanel.SetActive(false);
    }

    public void Destroy()
    {
        if(_targetBuildingObject == null) return;
        _targetBuildingObject.ReturnMaterialsToInventory();
        Destroy(_targetBuildingObject.gameObject);
        _targetBuildingObject = null;
        _hammerPanel.SetActive(false);
    }
}
