using Storage_Boxes;
using UnityEngine;

public class ActiveInvetoriesHandler : MonoBehaviour
{
    public static ActiveInvetoriesHandler singleton { get; private set; }

    [SerializeField] private Storage _playerInventory;
    private Storage _activeInventory;

    private void Awake()
        => singleton = this;

    public void AddActiveInventory(Storage storage)
        => _activeInventory = storage; 

    private void ResetActiveInventory()
        => _activeInventory = null;
    
    public void HandleCell(ItemDisplayer itemDisplayer)
    { 
        var itemInventory = itemDisplayer.PreviousCell.Inventory;
        if (itemInventory == null || _activeInventory == null || _playerInventory == null) return;
        if (itemInventory == _playerInventory)
        {
            _playerInventory.ResetItemServerRpc(itemDisplayer.PreviousCell.Index);
            _activeInventory.AddItemToDesiredSlotServerRpc(itemDisplayer.InventoryCell.Item.Id, itemDisplayer.InventoryCell.Count);
        }
        else
        {
            _activeInventory.ResetItemServerRpc(itemDisplayer.PreviousCell.Index);
            _playerInventory.AddItemToDesiredSlotServerRpc(itemDisplayer.InventoryCell.Item.Id, itemDisplayer.InventoryCell.Count);
        }
        Destroy(itemDisplayer.gameObject);
    }
}