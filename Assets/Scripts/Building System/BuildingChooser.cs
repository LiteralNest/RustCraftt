using Building_System.Blue_Prints;
using UnityEngine;

namespace Building_System
{
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

        public void ChooseBuilding(BluePrint buildingBluePrint)
        {
            var instance = Instantiate(buildingBluePrint);
            _buildingDragger.SetCurrentPref(instance);
            CharacterUIHandler.singleton.ActivateBuildingChoosingPanel(false);
            CharacterUIHandler.singleton.ActivateBuildingStaffPanel(true);
        }
    }
}