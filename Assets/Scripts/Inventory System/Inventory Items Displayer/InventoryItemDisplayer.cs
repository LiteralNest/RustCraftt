using System.Threading.Tasks;
using Player_Controller;
using Storage_System;
using UI;
using UnityEngine.EventSystems;

namespace Inventory_System.Inventory_Items_Displayer
{
    public class InventoryItemDisplayer : ItemDisplayer, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        protected Storage _storage;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!GlobalValues.CanDragInventoryItems) return;
            PreviousCell.ResetItemWhileDrag();
            if (_countText != null)
                _countText.gameObject.SetActive(false);
            transform.SetParent(PlaceForInventoryItems.Singleton.transform);
            PlayerNetCode.Singleton.ResourcesDropper.InventoryItemDisplayer = this;
            _itemIcon.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!GlobalValues.CanDragInventoryItems) return;
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!GlobalValues.CanDragInventoryItems) return;
            if (_countText != null)
                _countText.gameObject.SetActive(true);
            transform.position = PreviousCell.transform.position;
            // if (InventoryCell.Count == 0)
            //     _storage.ResetItemServerRpc(PreviousCell.Index);
            // else
            //     _storage.SetItemServerRpc(PreviousCell.Index,
            //         new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count, InventoryCell.Hp, InventoryCell.Ammo));
            transform.SetParent(PreviousCell.transform);
            _itemIcon.raycastTarget = true;
        }

        public void Init(SlotDisplayer slot, InventoryCell cell, Storage storage)
        {
            _storage = storage;
            PreviousCell = slot;

            var cellTransform = slot.transform;
            SetPosition();
            transform.SetParent(cellTransform);

            InventoryCell = new InventoryCell(cell);
            DisplayData();
        }

        public override void SetNewCell(SlotDisplayer slotDisplayer)
        {
            base.SetNewCell(slotDisplayer);
            _storage = slotDisplayer.Inventory;
        }
    }
}