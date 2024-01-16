using UnityEngine;
using UnityEngine.UI;

namespace Building_System.Upgrading.UI
{
    public class BuildingUpgradeView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _demolishButton;
        [SerializeField] private Button _pickUpButton;
        [SerializeField] private Button _repaitButton;

        private void DisplayButton(bool value, Button targetButton)
            => targetButton.gameObject.SetActive(value);

        public void DisplayButtons(bool canBeUpgraded, bool canBeDestroyed, bool canBeRepaired, bool canBePickUp)
        {
            DisplayButton(canBeUpgraded, _upgradeButton);
            DisplayButton(canBeDestroyed, _demolishButton);
            DisplayButton(canBeRepaired, _repaitButton);
            DisplayButton(canBePickUp, _pickUpButton);
        }
    }
}
