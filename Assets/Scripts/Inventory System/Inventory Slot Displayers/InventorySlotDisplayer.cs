using Inventory_System.Inventory_Items_Displayer;
using Storage_System;
using UnityEngine.EventSystems;

public class InventorySlotDisplayer : SlotDisplayer
{
    protected override void Drop(PointerEventData eventData)
    {
        var itemDisplayer = eventData.pointerDrag.GetComponent<InventoryItemDisplayer>();
        Storage cachedStorage = itemDisplayer.PreviousCell.Inventory;
        int cachedIndex = itemDisplayer.PreviousCell.Index;
        if (TrySetItem(itemDisplayer))
        {
            cachedStorage.ResetItemServerRpc(cachedIndex);
            return;
        }

        itemDisplayer.SetPosition();
    }
}