using Player_Controller;
using Storage_System;
using UnityEngine;

namespace Inventory_System
{
    public class ActiveInvetoriesHandler : MonoBehaviour
    {
        public static ActiveInvetoriesHandler singleton { get; private set; }

        [SerializeField] private Storage _playerInventory;
        private Storage _activeInventory;

        private void Awake()
            => singleton = this;

        public void AddActiveInventory(Storage storage)
            => _activeInventory = storage; 
    
        public void HandleCell(ItemDisplayer itemDisplayer)
        { 
            var itemInventory = itemDisplayer.PreviousCell.Inventory;
            if (itemInventory == null || _activeInventory == null || _playerInventory == null) return;
            if (itemInventory == _playerInventory)
            {
                int cellIdex = _activeInventory.GetAvailableCellIndexForMovingItem(itemDisplayer.InventoryCell.Item);
                if(cellIdex == -1) return;
                _playerInventory.ResetItemServerRpc(itemDisplayer.PreviousCell.Index, (int)PlayerNetCode.Singleton.OwnerClientId);
                var inventoryCell = itemDisplayer.InventoryCell;
                _activeInventory.SetItemServerRpc(cellIdex, new CustomSendingInventoryDataCell(inventoryCell.Item.Id, inventoryCell.Count, inventoryCell.Hp, inventoryCell.Ammo));
            }
            else
            {
                _activeInventory.ResetItemServerRpc(itemDisplayer.PreviousCell.Index, (int)PlayerNetCode.Singleton.OwnerClientId);
                _playerInventory.AddItemToDesiredSlotServerRpc(itemDisplayer.InventoryCell.Item.Id, itemDisplayer.InventoryCell.Count, itemDisplayer.InventoryCell.Ammo);
            }
            Destroy(itemDisplayer.gameObject);
        }
    }
}