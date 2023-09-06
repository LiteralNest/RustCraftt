using UnityEngine;

[RequireComponent(typeof(BuildingDragger))]
public class BuildingChooser : MonoBehaviour
{
    [Header("Attached scripts")] [SerializeField]
    private BuildingDragger _buildingDragger;

    private void Start()
    {
        if (_buildingDragger == null)
            _buildingDragger = GetComponent<BuildingDragger>();
    }

    public void ChooseBuilding(BuildingBluePrint buildingBluePrint)
    {
        var instance = Instantiate(buildingBluePrint);
        _buildingDragger.SetCurrentPref(instance);
        GlobalEventsContainer.ShouldDisplayBuildingChoosePanel?.Invoke(false);
        GlobalEventsContainer.ShouldDisplayBuildingStaff?.Invoke(true);
    }
}