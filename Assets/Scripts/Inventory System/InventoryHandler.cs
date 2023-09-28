using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public static InventoryHandler singleton { get; set; }
    
    [field:SerializeField] public PlayerNetCode PlayerNetCode { get; private set; }
    [field:SerializeField] public CharacterStats Stats { get; private set; }
    [field:SerializeField] public LootBoxSlotsContainer LootBoxSlotsContainer { get; private set; }
    [field:SerializeField] public SlotsContainer InventorySlotsContainer { get; private set; }
    [field:SerializeField] public InventorySlotsDisplayer InventorySlotsDisplayer { get; private set; }
    [field:SerializeField] public PlayerObjectsPlacer PlayerObjectsPlacer { get; private set; }
    [field:SerializeField] public BuildingChooser BuildingChooser { get; private set; }
    [Header("UI")] 
    [SerializeField] private GameObject _mainButtonsPanel;
    [SerializeField] private GameObject _armorPanel;
    [SerializeField] private GameObject _lootBoxPanel;
    [SerializeField] private GameObject _inventoryPanel;
    
    public Item ActiveItem { get; private set; }

    private void Awake()
        => singleton = this;
    
    public void HandleInventory(bool isOpen)
    {
        _mainButtonsPanel.SetActive(!isOpen);
        _inventoryPanel.SetActive(isOpen);
        GlobalValues.CanDragInventoryItems = isOpen;
        GlobalValues.CanLookAround = !isOpen;
    }

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
        _lootBoxPanel.SetActive(true);
    }

    public void SetActiveItem(Item item)
    {
        ActiveItem = item;
    }
}
