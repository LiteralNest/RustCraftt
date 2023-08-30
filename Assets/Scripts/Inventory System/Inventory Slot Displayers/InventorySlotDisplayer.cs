using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotDisplayer : SlotDisplayer
{
    public override void ClearItem()
    {
        Inventory.ResetItemAt(Index);
        ItemDisplayer = null;
    }
 
    protected override void AddItem(InventoryItemDisplayer item)
    {
        ItemDisplayer = item;
        item.Init(this, Inventory);
        Inventory.SetItemAt(Index, item.InventoryCell);
    }

    protected override void Drop(PointerEventData eventData)
    {
        InventoryItemDisplayer newItemDisplayer = eventData.pointerDrag.GetComponent<InventoryItemDisplayer>();
        if(TryAddToFreeSlot(newItemDisplayer)) return;

        int togetherCount = ItemDisplayer.InventoryCell.Count + newItemDisplayer.InventoryCell.Count;
        if (ItemDisplayer.InventoryCell.Item.Id == newItemDisplayer.InventoryCell.Item.Id)
        {
            if (togetherCount >= ItemDisplayer.InventoryCell.Item.StackCount)
            {
                int diff = togetherCount - newItemDisplayer.InventoryCell.Item.StackCount;
                newItemDisplayer.SetCount(diff);
                ItemDisplayer.SetCount(togetherCount - diff);
                return;
            }

            ItemDisplayer.AddCount(newItemDisplayer.InventoryCell.Count);
            newItemDisplayer.MinusCount(newItemDisplayer.InventoryCell.Count);
        }
    }
}