using TMPro;
using UnityEngine;

public class BuildingUpgrader : MonoBehaviour
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private TMP_Text _upgradeText;
    [SerializeField] private Camera _targetCamera;
    [SerializeField] private bool _canUpgrade;
    
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
        _upgradeText.gameObject.SetActive(false);
        _targetBuildingObject = GetTargetBuildingObject();
        if(!CanUpgradeObject()) return;
        _upgradeText.gameObject.SetActive(true);
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
    }
}
