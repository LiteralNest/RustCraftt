using Items_System.Items;
using Player_Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory_System.ItemInfo
{
    public class ItemInfoHandler : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private GameObject _displayingPanel;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _icon;

        [Header("Buttons")] [SerializeField] private GameObject _eatButton;
        
        private SlotDisplayer _currentSlotDisplayer;

        public void ResetPanel()
            => _displayingPanel.SetActive(false);

        public void AssignItem(SlotDisplayer slotDisplayer)
        {
            _displayingPanel.SetActive(true);
            _currentSlotDisplayer = slotDisplayer;
            var cell = slotDisplayer.ItemDisplayer.InventoryCell;
            _titleText.text = cell.Item.Name;
            _descriptionText.text = cell.Item.Description;
            _icon.sprite = cell.Item.Icon;
            if (cell.Item is Food)
                _eatButton.SetActive(true);
            else
                _eatButton.SetActive(false);
        }

        public void Eat()
        {
            var cell = _currentSlotDisplayer.ItemDisplayer.InventoryCell;
            var food = cell.Item as Food;
            if (food == null) return;
            if (cell.Count == 1) ResetPanel();
            food.Click(_currentSlotDisplayer);
        }

        public void Drop()
        {
            var cell = _currentSlotDisplayer.ItemDisplayer.InventoryCell;
            var camera = Camera.main.transform;
            InstantiatingItemsPool.sigleton.SpawnDropableObjectServerRpc(cell.Item.Id, cell.Count,
                camera.transform.position + camera.forward * 1.5f);
            InventoryHandler.singleton.CharacterInventory.RemoveItemCountFromSlotServerRpc(_currentSlotDisplayer.Index,
                cell.Item.Id, cell.Count);
            ResetPanel();
        }
    }
}