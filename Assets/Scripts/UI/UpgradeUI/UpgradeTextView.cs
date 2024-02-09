using Inventory_System;
using TMPro;
using UnityEngine;

namespace UI.UpgradeUI
{
    public class UpgradeTextView : MonoBehaviour
    {
        [Header("Panel")] [SerializeField] private GameObject _displayPanel;
        [Header("Main Text")] [SerializeField] private TMP_Text _mainText;

        [SerializeField] private Color _enoughMaterialsColor;
        [SerializeField] private Color _notEnoughMaterialsColor;

        public void DisplayText(string upgradeTitle, InventoryCell upgradeCellCost)
        {
            HandleText(true);
            var inventoryCount = InventoryHandler.singleton.CharacterInventory.GetItemCount(upgradeCellCost.Item.Id);
            var targetColor = inventoryCount < upgradeCellCost.Count ? _notEnoughMaterialsColor : _enoughMaterialsColor;
            var htmlColor = "#" + ColorUtility.ToHtmlStringRGBA(targetColor);
            _mainText.text =
                $"UPGRADE ({upgradeTitle} <color={htmlColor}> {inventoryCount} / {upgradeCellCost.Count} </color>)";
        }

        public void HandleText(bool value)
            => _displayPanel.SetActive(value);
    }
}