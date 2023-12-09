using Inventory_System.Inventory_Items_Displayer;
using Storage_System;
using UnityEngine.EventSystems;

public class InventorySlotDisplayer : SlotDisplayer
{
    protected override void Drop(PointerEventData eventData)
    {
        var itemDisplayer = eventData.pointerDrag.GetComponent<InventoryItemDisplayer>();
        if (TrySetItem(itemDisplayer))
        {
            if(itemDisplayer != null)
                Destroy(itemDisplayer.gameObject);
            return;
        }
        itemDisplayer.SetPosition();
    }
}