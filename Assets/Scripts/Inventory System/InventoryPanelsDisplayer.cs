using Events;
using Player_Controller;
using UnityEngine;

namespace Inventory_System
{
    public class InventoryPanelsDisplayer : MonoBehaviour
    {
        [Header("Attached Scripts")] [SerializeField]
        private InventoryHandler _inventoryHandler;

        [Header("UI")] [SerializeField] private GameObject _mainButtonsPanel;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private GameObject _characterPreview;
        [Space] [Space] [SerializeField] private GameObject _backPackPanel;
        [SerializeField] private GameObject _workbenchPanel;
        [Space] [Space] [SerializeField] private GameObject _inventoryCellsPanel;
        [SerializeField] private GameObject _craftPanel;

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
            PlayerNetCode.Singleton.ActiveInvetoriesHandler.AddActiveInventory(null);
        }

        public void OpenInventory(bool deactivateCharacterView)
        {
            CurrentInventoriesHandler.Singleton.HandleCurrentStoragePanel(true);
            ResetInventories();
            HandleInventory(true);
            if (deactivateCharacterView)
                HandleCharacterPreview(false);
        }

        public void HandleCharacterPreview(bool value)
            => _characterPreview.SetActive(value);

        public void CloseWorkbenchPanel()
        {
            _workbenchPanel.SetActive(false);
        }

        public void OpenWorkbenchPanel()
        {
            OpenInventory(true);
            _workbenchPanel.SetActive(true);
        }

        public void OpenCraft()
            => CurrentInventoriesHandler.Singleton.HandleCurrentStoragePanel(false);

        public void OpenInventory()
            => CurrentInventoriesHandler.Singleton.HandleCurrentStoragePanel(true);
    }
}