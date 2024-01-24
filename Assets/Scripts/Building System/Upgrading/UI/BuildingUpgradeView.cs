using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Building_System.Upgrading.UI
{
    public class BuildingUpgradeView : MonoBehaviour
    {
        [Header("Attached scripts")] [SerializeField]
        private UpgradeView _upgradeView;

        [SerializeField] private GameObject _upgradeCyclePanel;

        [Header("Buttons")] [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _demolishButton;
        [SerializeField] private Button _pickUpButton;
        [SerializeField] private Button _repaitButton;

        private void DisplayButton(bool value, Button targetButton)
            => targetButton.gameObject.SetActive(value);

        public void DisplayCycle(bool value)
        {
            if(_upgradeCyclePanel.gameObject.activeSelf == value) return;
            _upgradeCyclePanel.gameObject.SetActive(value);
        }

        public void DisplayButtons(bool canBeUpgraded, bool canBeDestroyed, bool canBeRepaired, bool canBePickUp,
            List<InventoryCell> cells = null, int level = 0)
        {
            if(cells == null)
                DisplayCycle(false);
            if (_upgradeCyclePanel.activeSelf)
            {
                DisplayButton(false, _upgradeButton);
                DisplayButton(false, _demolishButton);
                DisplayButton(false, _repaitButton);
                DisplayButton(false, _pickUpButton);
                return;
            }
            DisplayButton(canBeUpgraded, _upgradeButton);
            DisplayButton(canBeDestroyed, _demolishButton);
            DisplayButton(canBeRepaired, _repaitButton);
            DisplayButton(canBePickUp, _pickUpButton);
            if (cells != null)
                _upgradeView.DisplayUpgradeCells(cells, level);
        }
    }
}