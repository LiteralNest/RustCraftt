using System.Threading.Tasks;
using UnityEngine.EventSystems;

public class InventoryItemDisplayer : ItemDisplayer, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected SlotsContainer _slotsContainer;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GlobalValues.CanDragInventoryItems) return;
        _slotsContainer.ResetCell(PreviousCell.Index);
        PreviousCell.ResetItem();
        _countText.gameObject.SetActive(false);
        transform.SetParent(transform.root);
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
        _countText.gameObject.SetActive(true);
        transform.position = PreviousCell.transform.position;
        transform.SetParent(PreviousCell.transform);
        _itemIcon.raycastTarget = true;
    }

    public void Init(SlotDisplayer slot, InventoryCell cell, SlotsContainer slotsContainer)
    {
        _slotsContainer = slotsContainer;
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
        _slotsContainer = slotDisplayer.Inventory;
        if (!_slotsContainer) return;
        _slotsContainer.AddCell(slotDisplayer.Index, InventoryCell);
        GlobalEventsContainer.InventoryDataShouldBeSaved?.Invoke(_slotsContainer.Cells);
    }

    public override int StackCount(int addedCount, SlotDisplayer slotDisplayer)
    {
        var res = base.StackCount(addedCount, slotDisplayer);
        _slotsContainer.AddCell(slotDisplayer.Index, InventoryCell);
        GlobalEventsContainer.InventoryDataShouldBeSaved?.Invoke(_slotsContainer.Cells);
        RedisplayInventrory();
        return res;
    }

    private async void RedisplayInventrory()
    {
        await Task.Delay(100);
        GlobalEventsContainer.ShouldDisplayInventoryCells?.Invoke();
    }
}
