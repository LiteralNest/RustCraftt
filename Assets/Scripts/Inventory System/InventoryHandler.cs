using ArmorSystem.Backend;
using Player_Controller;
using Storage_Boxes;
using Unity.Netcode;
using UnityEngine;

public class InventoryHandler : NetworkBehaviour
{
    public static InventoryHandler singleton { get; set; }

    [field: SerializeField] public PlayerNetCode PlayerNetCode { get; private set; }
    [field: SerializeField] public CharacterStats Stats { get; private set; }
    [field: SerializeField] public PlayerObjectsPlacer PlayerObjectsPlacer { get; private set; }
    [field: SerializeField] public BuildingChooser BuildingChooser { get; private set; }
    [field: SerializeField] public InHandObjectsContainer InHandObjectsContainer { get; private set; }
    [field: SerializeField] public ArmorsContainer ArmorsContainer { get; private set; }

    [field: SerializeField] public SlotsDisplayer ShotGunSlotsDisplayer { get; private set; }
    [field: SerializeField] public InventorySlotsDisplayer InventorySlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer ToolClipboardSlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer LargeStorageSlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer LootBoxSlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer CampFireSlotsDisplayer { get; private set; }
    [field: SerializeField] public SlotsDisplayer RecyclerSlotsDisplayer { get; private set; }
    [field: SerializeField] public Storage CharacterInventory { get; private set; }


    [Header("UI")] [SerializeField] private GameObject _mainButtonsPanel;
    [SerializeField] private GameObject _armorPanel;
    [SerializeField] private GameObject _lootBoxPanel;
    [SerializeField] private GameObject _largeStoragePanel;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _campFirePanel;
    [SerializeField] private GameObject _recyclerPanel;
    [SerializeField] private GameObject _toolClipboardPanel;
    [SerializeField] private GameObject _shotGunPanel;

    public Item ActiveItem { get; private set; }
    public SlotDisplayer ActiveSlotDisplayer { get; set; }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            singleton = this;
        base.OnNetworkSpawn();
    }

    public void HandleInventory(bool isOpen)
    {
        _mainButtonsPanel.SetActive(!isOpen);
        _inventoryPanel.SetActive(isOpen);
        GlobalValues.CanDragInventoryItems = isOpen;
        GlobalValues.CanLookAround = !isOpen;
    }

    public void DisplayInventoryCells()
        => CharacterInventory.Open(this);

    public void OpenArmorPanel()
    {
        HandleInventory(true);
        _armorPanel.SetActive(true);
        _lootBoxPanel.SetActive(false);
    }

    public void OpenLootBoxPanel()
    {
        HandleInventory(true);
        _armorPanel.SetActive(false);
        _campFirePanel.SetActive(false);
        _largeStoragePanel.SetActive(false);
        _toolClipboardPanel.SetActive(false);
        _shotGunPanel.SetActive(false);
        _lootBoxPanel.SetActive(true);
    }

    public void OpenCampFirePanel()
    {
        HandleInventory(true);
        _armorPanel.SetActive(false);
        _lootBoxPanel.SetActive(false);
        _largeStoragePanel.SetActive(false);
        _toolClipboardPanel.SetActive(false);
        _shotGunPanel.SetActive(false);
        _campFirePanel.SetActive(true);
    }

    public void OpenRecyclerPanel()
    {
        HandleInventory(true);
        _armorPanel.SetActive(false);
        _lootBoxPanel.SetActive(false);
        _largeStoragePanel.SetActive(false);
        _toolClipboardPanel.SetActive(false);
        _shotGunPanel.SetActive(false);
        _recyclerPanel.SetActive(true);
    }

    public void OpenLargeChestPanel()
    {
        HandleInventory(true);
        _armorPanel.SetActive(false);
        _lootBoxPanel.SetActive(false);
        _recyclerPanel.SetActive(false);
        _toolClipboardPanel.SetActive(false);
        _shotGunPanel.SetActive(false);
        _largeStoragePanel.SetActive(true);
    }

    public void OpenClipBoardPanel()
    {
        HandleInventory(true);
        _armorPanel.SetActive(false);
        _lootBoxPanel.SetActive(false);
        _recyclerPanel.SetActive(false);
        _largeStoragePanel.SetActive(false);
        _shotGunPanel.SetActive(false);
        _toolClipboardPanel.SetActive(true);
    }

    public void OpenShotGunPanel()
    {
        HandleInventory(true);
        _armorPanel.SetActive(false);
        _lootBoxPanel.SetActive(false);
        _recyclerPanel.SetActive(false);
        _largeStoragePanel.SetActive(false);
        _toolClipboardPanel.SetActive(false);
        _shotGunPanel.SetActive(true);
    }

    public void SetActiveItem(Item item)
    {
        ActiveItem = item;
    }
}