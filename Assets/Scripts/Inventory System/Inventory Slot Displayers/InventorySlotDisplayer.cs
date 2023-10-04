using UnityEngine.EventSystems;

public class InventorySlotDisplayer : SlotDisplayer
{
    protected override void Drop(PointerEventData eventData)
    {
        var itemDisplayer = eventData.pointerDrag.GetComponent<InventoryItemDisplayer>();
        if(TrySetItem(itemDisplayer)) return;
        itemDisplayer.SetPosition();
    }
}