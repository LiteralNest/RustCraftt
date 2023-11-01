using UnityEngine;

public class ActiveInvetoriesHandler : MonoBehaviour
{
    public static ActiveInvetoriesHandler singleton { get; private set; }

    [SerializeField] private SlotsContainer _playerInventory;
    private SlotsContainer _activeInventory;

    private void Awake()
        => singleton = this;

    private void OnEnable()
        => GlobalEventsContainer.ShouldResetCurrentInventory += ResetActiveInventory;

    private void OnDisable()
        => GlobalEventsContainer.ShouldResetCurrentInventory -= ResetActiveInventory;

    public void AddActiveInventory(SlotsContainer slotsContainer)
        => _activeInventory = slotsContainer; 

    private void ResetActiveInventory()
        => _activeInventory = null;
    
    public void HandleCell(ItemDisplayer itemDisplayer)
    { 
        var itemInventory = itemDisplayer.PreviousCell.Inventory;
        if (itemInventory == null || _activeInventory == null || _playerInventory == null) return;
        if (itemInventory == _playerInventory)
        {
            _playerInventory.ResetCellAndSendData(itemDisplayer.PreviousCell.Index);
            _activeInventory.AddItemToDesiredSlot(itemDisplayer.InventoryCell.Item, itemDisplayer.InventoryCell.Count);
        }
        else
        {
            _activeInventory.ResetCellAndSendData(itemDisplayer.PreviousCell.Index);
            _playerInventory.AddItemToDesiredSlot(itemDisplayer.InventoryCell.Item, itemDisplayer.InventoryCell.Count);
        }
        Destroy(itemDisplayer.gameObject);
    }
}