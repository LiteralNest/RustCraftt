using Inventory_System;
using UnityEngine.EventSystems;

public class InventorySlotDisplayer : SlotDisplayer, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if(ItemDisplayer != null) return;
        ItemInfoDisplayer.Singleton.DisplayItemInfo(null);
    }
}