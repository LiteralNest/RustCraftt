using Inventory_System.Inventory_Items_Displayer;
using Inventory_System.Inventory_Slot_Displayers;
using Player_Controller;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory_System
{
    public class ItemPreviewItemDisplayer : InventoryItemDisplayer
    {
        private SlotDisplayer _cachedSlotDisplayer;
        private Transform _cachedParrent;
        private Vector3 _cachedPosition;

        public void Init(SlotDisplayer slotDisplayer)
        {
            _cachedSlotDisplayer = slotDisplayer;
        }

        public override void DoAfterMovingItemOut()
        {
            _cachedSlotDisplayer.Inventory.RemoveItemCountFromSlotServerRpc(_cachedSlotDisplayer.Index,
                InventoryCell.Item.Id, InventoryCell.Count);
            PlayerNetCode.Singleton.ItemInfoHandler.ResetPanel();
        }

        public override void PointerClicked()
        {
        }

        public override void BeginDrag(PointerEventData eventData)
        {
            _cachedPosition = transform.position;
            _cachedParrent = transform.parent;
            if (!GlobalValues.CanDragInventoryItems) return;
            _countText.gameObject.SetActive(false);
            transform.SetParent(PlaceForInventoryItems.Singleton.transform);
            PlayerNetCode.Singleton.ResourcesDropper.InventoryItemDisplayer = this;
            _itemIcon.raycastTarget = false;
        }

        public override void Drag(PointerEventData eventData)
        {
            if (!GlobalValues.CanDragInventoryItems) return;
            transform.position = eventData.position;
        }

        public override void EndDrag(PointerEventData eventData)
        {
            if (!GlobalValues.CanDragInventoryItems) return;
            _countText.gameObject.SetActive(true);
            transform.position = _cachedPosition;
            transform.SetParent(_cachedParrent);
            _itemIcon.raycastTarget = true;
        }
    }
}