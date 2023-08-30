using UnityEngine.EventSystems;

public class LootBoxSlotDisplayer : SlotDisplayer
{
    public override void ClearItem()
    {
        ItemDisplayer = null;
    }

    protected override void Drop(PointerEventData eventData)
    {
        InventoryItemDisplayer newItemDisplayer = eventData.pointerDrag.GetComponent<InventoryItemDisplayer>();
        if(!TryAddToFreeSlot(eventData.pointerDrag.GetComponent<InventoryItemDisplayer>())) return;
        
    }

    protected override void AddItem(InventoryItemDisplayer item)
    {
        Inventory.SetItemAt(Index, item.InventoryCell);
    }
}