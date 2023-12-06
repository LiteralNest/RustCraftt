using UnityEngine;

namespace Inventory_System
{
    public class InventoryPanelsDisplayer : MonoBehaviour
    {
        public static InventoryPanelsDisplayer singleton { get; set; }
        
        [Header("Attached Scripts")]
        [SerializeField] private InventoryHandler _inventoryHandler;
        
        [Header("UI")] 
        [SerializeField] private GameObject _mainButtonsPanel;
        [SerializeField] private GameObject _inventoryPanel;
        [Space]
        [Space]
        [SerializeField] private GameObject _armorPanel;
        [SerializeField] private GameObject _lootBoxPanel;
        [SerializeField] private GameObject _largeStoragePanel;
        [SerializeField] private GameObject _campFirePanel;
        [SerializeField] private GameObject _furnacePanel;
        [SerializeField] private GameObject _recyclerPanel;
        [SerializeField] private GameObject _toolClipboardPanel;
        [SerializeField] private GameObject _shotGunPanel;
        [SerializeField] private GameObject _backPackPanel;
        [SerializeField] private GameObject _workbenchPanel;

        private void Awake()
            => singleton = this;

        public void DisplayInventoryCells()
            => _inventoryHandler.DisplayInventoryCells();
        
        public void HandleInventory(bool isOpen)
        {
            _mainButtonsPanel.SetActive(!isOpen);
            _inventoryPanel.SetActive(isOpen);
            GlobalValues.CanDragInventoryItems = isOpen;
            GlobalValues.CanLookAround = !isOpen;
        }

        public void ResetInventories()
        {
            _armorPanel.SetActive(false);
            _lootBoxPanel.SetActive(false);
            _largeStoragePanel.SetActive(false);
            _campFirePanel.SetActive(false);
            _recyclerPanel.SetActive(false);
            _toolClipboardPanel.SetActive(false);
            _shotGunPanel.SetActive(false);
            _backPackPanel.SetActive(false);
        }
        
        public void ClosePanels()
        {
            _inventoryPanel.SetActive(false);
            ResetInventories();
        }

        public void OpenArmorPanel()
        {
            ResetInventories();
            HandleInventory(true);
            _armorPanel.SetActive(true);
        }

        public void OpenLootBoxPanel()
        {
            ResetInventories();
            HandleInventory(true);
            _lootBoxPanel.SetActive(true);
        }

        public void OpenLargeChestPanel()
        {
            ResetInventories();
            HandleInventory(true);
            _largeStoragePanel.SetActive(true);
        }

        public void OpenWorkbenchPanel()
        {
            ResetInventories();
            HandleInventory(true);
            _workbenchPanel.SetActive(true);
        }
        
        public void OpenCampFirePanel()
        {
            ResetInventories();
            HandleInventory(true);
            _campFirePanel.SetActive(true);
        }

        public void OpenBackPackPanel()
        {
            ResetInventories();
            HandleInventory(true);
            _backPackPanel.SetActive(true);
        }
        
        public void OpenRecyclerPanel()
        {
            ResetInventories();
            HandleInventory(true);
            _recyclerPanel.SetActive(true);
        }

        public void OpenClipBoardPanel()
        {
            ResetInventories();
            HandleInventory(true);
            _toolClipboardPanel.SetActive(true);
        }

        public void OpenShotGunPanel()
        {
            ResetInventories();
            HandleInventory(true);
            _shotGunPanel.SetActive(true);
        }

        public void OpenFurnacePanel()
        {
            ResetInventories();
            HandleInventory(true);
            _furnacePanel.SetActive(true);
        }
    }
}