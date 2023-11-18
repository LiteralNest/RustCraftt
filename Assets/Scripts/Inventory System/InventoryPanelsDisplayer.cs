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
        [SerializeField] private GameObject _recyclerPanel;
        [SerializeField] private GameObject _toolClipboardPanel;
        [SerializeField] private GameObject _shotGunPanel;

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

        public void ClosePanels()
        {
            _armorPanel.SetActive(false);
            _lootBoxPanel.SetActive(false);
            _largeStoragePanel.SetActive(false);
            _inventoryPanel.SetActive(false);
            _campFirePanel.SetActive(false);
            _recyclerPanel.SetActive(false);
            _toolClipboardPanel.SetActive(false);
            _shotGunPanel.SetActive(false);
        }

        public void OpenArmorPanel()
        {
            HandleInventory(true);
            _armorPanel.SetActive(true);
        }

        public void OpenLootBoxPanel()
        {
            HandleInventory(true);
            _lootBoxPanel.SetActive(true);
        }

        public void OpenLargeChestPanel()
        {
            HandleInventory(true);
            _largeStoragePanel.SetActive(true);
        }

        public void OpenCampFirePanel()
        {
            HandleInventory(true);
            _campFirePanel.SetActive(true);
        }

        public void OpenRecyclerPanel()
        {
            HandleInventory(true);
            _recyclerPanel.SetActive(true);
        }

        public void OpenClipBoardPanel()
        {
            HandleInventory(true);
            _toolClipboardPanel.SetActive(true);
        }

        public void OpenShotGunPanel()
        {
            HandleInventory(true);
            _shotGunPanel.SetActive(true);
        }
    }
}