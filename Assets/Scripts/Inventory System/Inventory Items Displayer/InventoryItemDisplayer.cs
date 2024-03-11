using Events;
using Inventory_System.Inventory_Slot_Displayers;
using Player_Controller;
using UI;
using UnityEngine.EventSystems;

namespace Inventory_System.Inventory_Items_Displayer
{
    public class InventoryItemDisplayer : ItemDisplayer, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public virtual void BeginDrag(PointerEventData eventData)
        {
            if (!GlobalValues.CanDragInventoryItems) return;
            if (InventoryHandler.singleton.ActiveSlotDisplayer != null &&
                InventoryHandler.singleton.ActiveSlotDisplayer.Index == PreviousCell.Index)
                PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
            GlobalEventsContainer.InventoryItemDragged?.Invoke();
            PreviousCell.ResetItemWhileDrag();
            if (_countText != null)
                _countText.gameObject.SetActive(false);
            transform.SetParent(PlaceForInventoryItems.Singleton.transform);
            PlayerNetCode.Singleton.ResourcesDropper.InventoryItemDisplayer = this;
            _itemIcon.raycastTarget = false;
        }

        public virtual void Drag(PointerEventData eventData)
        {
            if (!GlobalValues.CanDragInventoryItems) return;
            transform.position = eventData.position;
        }

        public virtual void EndDrag(PointerEventData eventData)
        {
            // if (!GlobalValues.CanDragInventoryItems) return;
            // if (_countText != null)
            //     _countText.gameObject.SetActive(true);
            // transform.position = PreviousCell.transform.position;
            // transform.SetParent(PreviousCell.transform);
            // _itemIcon.raycastTarget = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
            => BeginDrag(eventData);

        public void OnDrag(PointerEventData eventData)
            => Drag(eventData);

        public void OnEndDrag(PointerEventData eventData)
            => EndDrag(eventData);

        public void Init(SlotDisplayer slot, InventoryCell cell)
        {
            PreviousCell = slot;

            var cellTransform = slot.transform;
            SetPosition();
            transform.SetParent(cellTransform);

            SetData(new InventoryCell(cell));
        }

        public void SetData(InventoryCell cell)
        {
            InventoryCell = cell;
            DisplayData();
        }
    }
}