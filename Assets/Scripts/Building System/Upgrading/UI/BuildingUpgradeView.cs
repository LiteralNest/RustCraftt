using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Building_System.Upgrading.UI
{
    public class BuildingUpgradeView : MonoBehaviour
    {
        [Header("Attached scripts")]
        [SerializeField] private BuildingUpgrader _buildingUpgrader;
        [SerializeField] private List<UpgradeCellView> _upgradeCells;
        [SerializeField] private GameObject _upgradeCyclePanel;

        [Header("Buttons")] [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _demolishButton;
        [SerializeField] private Button _pickUpButton;
        [SerializeField] private Button _repairButton;
        [SerializeField] private Button _selectUpgradingButton;

        private void Awake()
        {
            _selectUpgradingButton.onClick.RemoveAllListeners();
            _selectUpgradingButton.onClick.AddListener(() => DisplayCycle(true));
        }
        
        private void DisplayButton(bool value, Button targetButton)
            => targetButton.gameObject.SetActive(value);

        public void DisplayCycle(bool value)
        {
            if (_upgradeCyclePanel.gameObject.activeSelf == value) return;
            _upgradeCyclePanel.gameObject.SetActive(value);
            RedisplayCells();
        }

        public void RedisplayCells()
        {
            foreach (var cell in _upgradeCells)
            {
                cell.Init(_buildingUpgrader);
                cell.DisplayActive();
            }
        }

        public void DisplayButtons(bool canBeUpgraded, bool canBeDestroyed, bool canBeRepaired, bool canBePickUp,
            List<InventoryCell> cells = null)
        {
            if (_upgradeCyclePanel.activeSelf)
            {
                DisplayButton(false, _upgradeButton);
                DisplayButton(false, _demolishButton);
                DisplayButton(false, _repairButton);
                DisplayButton(false, _pickUpButton);
                return;
            }

            DisplayButton(canBeUpgraded, _upgradeButton);
            DisplayButton(canBeDestroyed, _demolishButton);
            DisplayButton(canBeRepaired, _repairButton);
            DisplayButton(canBePickUp, _pickUpButton);
        }
    }
}