using Events;
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
        [Space][Space]
        [SerializeField] private GameObject _inventoryCellsPanel;
        [SerializeField] private GameObject _craftPanel;

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
            _inventoryCellsPanel.SetActive(true);
            _craftPanel.SetActive(false);
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
            CurrentInventoriesHandler.Singleton.ResetCurrentStorage();
            ResetInventories();
        }

        public void OpenInventory(bool deactivateCharacterView)
        {
            CurrentInventoriesHandler.Singleton.HandleCurrentStoragePanel(true);
            ResetInventories();
            HandleInventory(true);
            if(deactivateCharacterView)
                _characterPreview.SetActive(false);
        }
        
        public void OpenWorkbenchPanel()
        {
            ResetInventories();
            HandleInventory(true);
            _workbenchPanel.SetActive(true);
        }

        public void OpenCraft()
            => CurrentInventoriesHandler.Singleton.HandleCurrentStoragePanel(false);
        
        public void OpenInventory()
            => CurrentInventoriesHandler.Singleton.HandleCurrentStoragePanel(true);
    }
}