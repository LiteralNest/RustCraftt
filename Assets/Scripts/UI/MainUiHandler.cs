using System.Threading.Tasks;
using UnityEngine;

public class MainUiHandler : MonoBehaviour
{
    public static MainUiHandler singleton { get; private set; }

    [SerializeField] private GameObject _buildingStaffPanel;
    [Space] [Space] [SerializeField] private GameObject _attackButton;
    [SerializeField] private GameObject _gatherButton;
    [SerializeField] private GameObject _buildingButton;
    [SerializeField] private GameObject _buildingChoosingPanel;
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private GameObject _pickUpButton;
    [SerializeField] private GameObject _reloadingButton;
    [SerializeField] private GameObject _placingPanel;
    [SerializeField] private GameObject _scopeButton;
    [SerializeField] private GameObject _throwingAimButton;
    [SerializeField] private GameObject _meleeThrowButton;
    [SerializeField] private GameObject _throwExplosiveButton;

      private void OnEnable()
    {
        GlobalEventsContainer.BuildingHammerActivated += ActivateUpgradeButton;
        GlobalEventsContainer.ShouldDisplayThrowButton += ActivateThrowButton;
    }
    private void OnDisable()
    {
        GlobalEventsContainer.BuildingHammerActivated -= ActivateUpgradeButton;
        GlobalEventsContainer.ShouldDisplayThrowButton -= ActivateThrowButton;
    }
    
    private void Awake()
        => singleton = this;

    public void ActivateMeleeThrowButton(bool value)
    {
        _meleeThrowButton.SetActive(value);
        Debug.Log("Attack");
    }

    public void ActivateThrowingButton(bool value)
        => _throwingAimButton.SetActive(value);

    public void ActivateScope(bool value)
        => _scopeButton.SetActive(value);

    public void ActivateBuildingStaffPanel(bool value)
        => _buildingStaffPanel.SetActive(value);

    public void ActivateBuildingChoosingPanel(bool value)
        => _buildingChoosingPanel.SetActive(value);

    public void ActivateBuildingButton(bool value)
        => _buildingButton.SetActive(value);

    public void ActivateUpgradeButton(bool value)
        => _upgradeButton.SetActive(value);

    public async void ActivateGatherButton(bool value)
        => _gatherButton.SetActive(value);

    public void ActivateAttackButton(bool value)
        => _attackButton.SetActive(value);

    public void ActivatePickupButton(bool value)
        => _pickUpButton.SetActive(value);

    public void ActivateReloadingButton(bool value)
        => _reloadingButton.SetActive(value);

    public void ActivatePlacingPanel(bool value)
        => _placingPanel.SetActive(value);

    public void ActivateThrowButton(bool value)
        => _throwExplosiveButton.SetActive(value);
}