using UnityEngine;

public class MainUiHandler : MonoBehaviour
{
    [SerializeField] private GameObject _attackButton;
    [SerializeField] private GameObject _buildingButton;
    [SerializeField] private GameObject _buildingChoosingPanel;
    [SerializeField] private GameObject _buildingStaffPanel;

    private void OnEnable()
    {
        GlobalEventsContainer.ShouldDisplayBuildingStaff += DisplayBuildingStaffPanel;
        GlobalEventsContainer.ShouldDisplayBuildingChoosePanel += DisplayBuildingChoosingPanel;
        GlobalEventsContainer.BluePrintActiveSelfSet += ActivateBuildingButton;
    }

    private void OnDisable()
    {
        GlobalEventsContainer.ShouldDisplayBuildingStaff -= DisplayBuildingStaffPanel;
        GlobalEventsContainer.ShouldDisplayBuildingChoosePanel -= DisplayBuildingChoosingPanel;
        GlobalEventsContainer.BluePrintActiveSelfSet -= ActivateBuildingButton;
    }
    
    private void DisplayBuildingStaffPanel(bool value)
        => _buildingStaffPanel.SetActive(value);

    private void DisplayBuildingChoosingPanel(bool value)
        => _buildingChoosingPanel.SetActive(value);

    private void ActivateBuildingButton(bool value)
    {
        _attackButton.SetActive(!value);
        _buildingButton.SetActive(value);
    }
}