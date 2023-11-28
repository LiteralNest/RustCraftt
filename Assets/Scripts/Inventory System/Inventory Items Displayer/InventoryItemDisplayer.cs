using System.Threading.Tasks;
using Storage_System;
using UnityEngine.EventSystems;

namespace Inventory_System.Inventory_Items_Displayer
{
    public class InventoryItemDisplayer : ItemDisplayer, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        protected Storage _storage;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!GlobalValues.CanDragInventoryItems) return;
            _storage.ResetItemServerRpc(PreviousCell.Index);
            PreviousCell.ResetItemWhileDrag();
            if(_countText != null)
                _countText.gameObject.SetActive(false);
            transform.SetParent(transform.root);
            ResourcesDropper.singleton.InventoryItemDisplayer = this;
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
            if(_countText != null)
                _countText.gameObject.SetActive(true);
            transform.position = PreviousCell.transform.position;
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
            if(_storage)
                _storage.ResetItemServerRpc(PreviousCell.Index);
            base.SetNewCell(slotDisplayer);
            _storage = slotDisplayer.Inventory;
            if (!_storage) return;
            _storage.SetItemServerRpc(slotDisplayer.Index, new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count, InventoryCell.Hp));
        }

        public override int StackCount(int addedCount, SlotDisplayer slotDisplayer)
        {
            var res = base.StackCount(addedCount, slotDisplayer);
            _storage.SetItemServerRpc(slotDisplayer.Index, new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count, InventoryCell.Hp));
            RedisplayInventrory();
            return res;
        }

        private async void RedisplayInventrory()
        {
            await Task.Delay(100);
            GlobalEventsContainer.ShouldDisplayInventoryCells?.Invoke();
        }
    }
}
