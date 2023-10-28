using UnityEngine.EventSystems;

public class LootBoxSlotDisplayer : SlotDisplayer
{
    protected override void Drop(PointerEventData eventData)
    {
        // InventoryItemDisplayer newItemDisplayer = eventData.pointerDrag.GetComponent<InventoryItemDisplayer>();
        // if(!TryAddToFreeSlot(eventData.pointerDrag.GetComponent<InventoryItemDisplayer>())) return;
        
    }
}