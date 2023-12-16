using UnityEngine;

namespace Inventory_System
{
    public class InventoryPanelsDisplayer : MonoBehaviour
    {
        public static InventoryPanelsDisplayer singleton { get; set; }

        [Header("Attached Scripts")] [SerializeField]
        private InventoryHandler _inventoryHandler;

        [Header("UI")] [SerializeField] private GameObject _mainButtonsPanel;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private GameObject _characterPreview;
        [Space] [Space] [SerializeField] private GameObject _backPackPanel;
        [SerializeField] private GameObject _workbenchPanel;

        private void Awake()
            => singleton = this;

        public void DisplayInventoryCells()
            => _inventoryHandler.DisplayInventoryCells();

        public void HandleInventory(bool isOpen)
        {
            if (!isOpen)
                GlobalEventsContainer.InventoryClosed?.Invoke();
            _mainButtonsPanel.SetActive(!isOpen);
            _inventoryPanel.SetActive(isOpen);
            GlobalValues.CanDragInventoryItems = isOpen;
            GlobalValues.CanLookAround = !isOpen;
        }

        public void ResetInventories()
        {
            _backPackPanel.SetActive(false);
        }

        public void ClosePanels()
        {
            _inventoryPanel.SetActive(false);
            ResetInventories();
        }

        public void OpenInventory(bool shouldDisplayArmorPanel)
        {
            ResetInventories();
            HandleInventory(true);
            _characterPreview.SetActive(shouldDisplayArmorPanel);
        }
        
        public void OpenWorkbenchPanel()
        {
            ResetInventories();
            HandleInventory(true);
            _workbenchPanel.SetActive(true);
        }
    }
}