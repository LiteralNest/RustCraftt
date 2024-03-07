using Events;
using Inventory_System.Inventory_Slot_Displayers;
using Items_System.Items.Food;
using Multiplayer;
using Storage_System;
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
        [SerializeField] private Transform _itemDisplayerPlace;
        [SerializeField] private Slider _slider;
        [SerializeField] private ItemPreviewItemDisplayer _itemDisplayerPrefab;
        
        [Header("Buttons")] 
        [SerializeField] private Button _eatButton;
        [SerializeField] private Button _dropButton;

        private SlotDisplayer _currentSlotDisplayer;
        private ItemPreviewItemDisplayer _targetItemDisplayer;
        private InventoryCell _cachedCell;

        private void OnEnable()
        {
            _slider.value = 1;
            GlobalEventsContainer.InventoryItemDragged += ResetPanel;
        }

        private void OnDisable()
        {
            GlobalEventsContainer.InventoryItemDragged -= ResetPanel;
            ResetPanel();
        }

        private void Start()
        {
            _slider.onValueChanged.AddListener(HandleSliderValue);
            _dropButton.onClick.AddListener(() =>
            {
                Drop();
                _displayingPanel.SetActive(false);
            });
            _eatButton.onClick.AddListener(Eat);
        }

        public void ResetPanel()
            => _displayingPanel.SetActive(false);

        public void AssignItem(SlotDisplayer slotDisplayer)
        {
            _displayingPanel.SetActive(true);
            _currentSlotDisplayer = slotDisplayer;
            var cell = slotDisplayer.ItemDisplayer.InventoryCell;
            _titleText.text = cell.Item.Name;
            _descriptionText.text = cell.Item.Description;
            GenerateItemDisplayer(slotDisplayer);
            if (cell.Item is Food)
                _eatButton.gameObject.SetActive(true);
            else
                _eatButton.gameObject.SetActive(false);
        }

        public void Eat()
        {
            var cell = _currentSlotDisplayer.ItemDisplayer.InventoryCell;
            var food = cell.Item as Food;
            if (food == null) return;
            if (cell.Count == 1) ResetPanel();
            food.Click(_currentSlotDisplayer);
            _targetItemDisplayer.SetData(new InventoryCell(cell.Item, cell.Count - 1));
        }

        private void Drop()
        {
            var cell = _targetItemDisplayer.InventoryCell;
            var camera = Camera.main.transform;
            InstantiatingItemsPool.sigleton.SpawnObjectServerRpc(
                new CustomSendingInventoryDataCell(cell.Item.Id, cell.Count, cell.Hp, cell.Ammo),
                camera.transform.position + camera.forward * 1.5f);
            _currentSlotDisplayer.Inventory.RemoveItemCountFromSlotServerRpc(_currentSlotDisplayer.Index,
                cell.Item.Id, cell.Count);
            ResetPanel();
        }

        private void GenerateItemDisplayer(SlotDisplayer slotDisplayer)
        {
            var cell = slotDisplayer.ItemDisplayer.InventoryCell;
            foreach (Transform child in _itemDisplayerPlace)
                Destroy(child.gameObject);
            _cachedCell = new InventoryCell(cell);
            _targetItemDisplayer = Instantiate(_itemDisplayerPrefab,
                _itemDisplayerPlace);
            _targetItemDisplayer.SetData(cell);
            _targetItemDisplayer.Init(slotDisplayer);
        }

        private void HandleSliderValue(float value)
        {
            var count = (int)(_cachedCell.Count * value);
            if (count <= 0) count = 1;
            _targetItemDisplayer.SetData(new InventoryCell(_targetItemDisplayer.InventoryCell.Item, count));
        }
    }
}