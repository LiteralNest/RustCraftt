using Events;
using Inventory_System.Inventory_Items_Displayer;
using Multiplayer;
using Storage_System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory_System
{
    public class ResourcesDropper : MonoBehaviour, IDropHandler
    {
        public InventoryItemDisplayer InventoryItemDisplayer { get; set; }

        [SerializeField] private float _droppingOffset = 1;
    
        private Vector3 GetFrontCameraPos()
        {
            var camera = Camera.main;
            Vector3 cameraPosition = camera.transform.position;
            Vector3 cameraForward = camera.transform.forward;
            return cameraPosition + (cameraForward * _droppingOffset);
        }
    
        private void TryResetInHandItem()
        {
            if (InventoryHandler.singleton.ActiveSlotDisplayer == null) return;
            if (InventoryItemDisplayer.PreviousCell == null) return;
            if(InventoryHandler.singleton.ActiveSlotDisplayer.Index == InventoryItemDisplayer.PreviousCell.Index)
                GlobalEventsContainer.OnCurrentItemDeleted?.Invoke();
        }
    
        public void OnDrop(PointerEventData eventData)
        {
            if (!InventoryItemDisplayer) return;
            TryResetInHandItem();
            var cell = InventoryItemDisplayer.PreviousCell;
            InventoryHandler.singleton.CharacterInventory.RemoveItemCountFromSlotServerRpc(cell.Index, InventoryItemDisplayer.InventoryCell.Item.Id, InventoryItemDisplayer.InventoryCell.Count);
            var addingCell = InventoryItemDisplayer.InventoryCell;
            InstantiatingItemsPool.sigleton.SpawnObjectServerRpc(new CustomSendingInventoryDataCell(addingCell.Item.Id, addingCell.Count, addingCell.Hp, addingCell.Ammo), GetFrontCameraPos());
            Destroy(InventoryItemDisplayer.gameObject);
            InventoryItemDisplayer = null;
        }
    }
}
