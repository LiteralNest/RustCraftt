using UnityEngine;
using UnityEngine.UI;

namespace Building_System.Upgrading.UI
{
    public class UpgradeCellView : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private Button _selectButton;
        [SerializeField] private GameObject _active;
        [SerializeField] private GameObject _unActive;
        [SerializeField] private int _level;

        private BuildingUpgrader _buildingUpgrader;

        public void Init(BuildingUpgrader buildingUpgrader, GameObject upgradeCyclePanel)
        {
            _buildingUpgrader = buildingUpgrader;
            _selectButton.onClick.RemoveAllListeners();
            _selectButton.onClick.AddListener(() =>
            {
                Select();
                upgradeCyclePanel.SetActive(false);
            });
        }

        public void DisplayActive()
        {
            bool isActive = _buildingUpgrader.SelectedLevel == _level;
            _active.SetActive(isActive);
            _unActive.SetActive(!isActive);
        }

        private void Select()
            => _buildingUpgrader.SetSelectedLevel(_level);
    }
}