using Storage_System;
using Unity.VisualScripting;
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

    public void ResetActiveInventory()
        => _activeInventory = null;

    public void DisplayItemsInActiveInventory(bool value)
    {
        if(_activeInventory == null) return;
        _activeInventory.HandleUi(value);
    }
    
    public void HandleCell(ItemDisplayer itemDisplayer)
    { 
        var itemInventory = itemDisplayer.PreviousCell.Inventory;
        if (itemInventory == null || _activeInventory == null || _playerInventory == null) return;
        if (itemInventory == _playerInventory)
        {
            int cellIdex = _activeInventory.GetAvailableCellIndexForMovingItem(itemDisplayer.InventoryCell.Item);
            if(cellIdex == -1) return;
            _playerInventory.ResetItemServerRpc(itemDisplayer.PreviousCell.Index);
            var inventoryCell = itemDisplayer.InventoryCell;
            _activeInventory.SetItemServerRpc(cellIdex, new CustomSendingInventoryDataCell(inventoryCell.Item.Id, inventoryCell.Count, inventoryCell.Hp, inventoryCell.Ammo));
        }
        else
        {
            _activeInventory.ResetItemServerRpc(itemDisplayer.PreviousCell.Index);
            _playerInventory.AddItemToDesiredSlotServerRpc(itemDisplayer.InventoryCell.Item.Id, itemDisplayer.InventoryCell.Count, itemDisplayer.InventoryCell.Ammo);
        }
        Destroy(itemDisplayer.gameObject);
    }
}