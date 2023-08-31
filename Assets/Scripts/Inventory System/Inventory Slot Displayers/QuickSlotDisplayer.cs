using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlotDisplayer : InventorySlotDisplayer, IPointerClickHandler
{
    [SerializeField] private InventoryHandler _inventoryHandler;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(GlobalValues.CanDragInventoryItems) return;
        var cell = ItemDisplayer.InventoryCell;
        if(cell == null) return;
        var item = cell.Item;
        if(item == null) return;
        item.Click(_inventoryHandler);
    }
}
