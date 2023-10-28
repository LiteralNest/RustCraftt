using UnityEngine.EventSystems;

public class InventorySlotDisplayer : SlotDisplayer
{
    protected override void Drop(PointerEventData eventData)
    {
        var itemDisplayer = eventData.pointerDrag.GetComponent<InventoryItemDisplayer>();
        if(TrySetItem(itemDisplayer)) return;
        itemDisplayer.SetPosition();
    }

    public override void Swap(ItemDisplayer itemDisplayer)
    {
        base.Swap(itemDisplayer);
        GlobalEventsContainer.ShouldDisplayInventoryCells?.Invoke();
    }
}