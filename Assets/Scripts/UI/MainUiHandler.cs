using System.Threading.Tasks;
using UnityEngine;

public class MainUiHandler : MonoBehaviour
{
    [SerializeField] private GameObject _attackButton;
    [SerializeField] private GameObject _gatherButton;
    [SerializeField] private GameObject _buildingButton;
    [SerializeField] private GameObject _buildingChoosingPanel;
    [SerializeField] private GameObject _buildingStaffPanel;
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private GameObject _pickUpButton;
    [SerializeField] private GameObject _reloadingButton;

    private void OnEnable()
    {
        GlobalEventsContainer.ShouldDisplayBuildingStaff += DisplayBuildingStaffPanel;
        GlobalEventsContainer.ShouldDisplayBuildingChoosePanel += DisplayBuildingChoosingPanel;
        GlobalEventsContainer.BluePrintActiveSelfSet += ActivateBuildingButton;
        GlobalEventsContainer.BuildingHammerActivated += ActivateUpgradeButton;
        GlobalEventsContainer.GatherButtonActivated += ActivateGatherButtonActivated;
        GlobalEventsContainer.AttackButtonActivated += ActivateAttackButton;
        GlobalEventsContainer.PickUpButtonActivated += ActivatePickupButton;
        GlobalEventsContainer.ShouldDisplayReloadingButton += ActivateReloadingButton;
    }

    private void OnDisable()
    {
        GlobalEventsContainer.ShouldDisplayBuildingStaff -= DisplayBuildingStaffPanel;
        GlobalEventsContainer.ShouldDisplayBuildingChoosePanel -= DisplayBuildingChoosingPanel;
        GlobalEventsContainer.BluePrintActiveSelfSet -= ActivateBuildingButton;
        GlobalEventsContainer.BuildingHammerActivated -= ActivateUpgradeButton;
        GlobalEventsContainer.GatherButtonActivated -= ActivateGatherButtonActivated;
        GlobalEventsContainer.AttackButtonActivated -= ActivateAttackButton;
        GlobalEventsContainer.PickUpButtonActivated -= ActivatePickupButton;
        GlobalEventsContainer.ShouldDisplayReloadingButton -= ActivateReloadingButton;
    }

    private void DisplayBuildingStaffPanel(bool value)
        => _buildingStaffPanel.SetActive(value);

    private void DisplayBuildingChoosingPanel(bool value)
        => _buildingChoosingPanel.SetActive(value);

    private void ActivateBuildingButton(bool value)
        => _buildingButton.SetActive(value);

    private void ActivateUpgradeButton(bool value)
        => _upgradeButton.SetActive(value);

    private async void ActivateGatherButtonActivated(bool value)
    {
        if(value)
            await Task.Delay(100);
        _gatherButton.SetActive(value);
    }

    private void ActivateAttackButton(bool value)
        => _attackButton.SetActive(value);

    private void ActivatePickupButton(bool value)
        => _pickUpButton.SetActive(value);

    private void ActivateReloadingButton(bool value)
        => _reloadingButton.SetActive(value);
}