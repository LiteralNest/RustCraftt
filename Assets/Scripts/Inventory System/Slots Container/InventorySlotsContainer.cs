using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsContainer : SlotsContainer
{
    [Header("Test")]
    [SerializeField] private Item _testItem;
    
    public override void AddItemToDesiredSlot(Item item, int count)
    {
        GlobalEventsContainer.InventoryItemAdded?.Invoke(new InventoryCell(item, count));
        base.AddItemToDesiredSlot(item, count);
        GlobalEventsContainer.InventoryDataChanged?.Invoke();
    }

    public override void DeleteSlot(Item item, int count)
    {
        base.DeleteSlot(item, count);
        GlobalEventsContainer.InventoryDataChanged?.Invoke();
    }
    
    [ContextMenu("Test Add Item")]
    private void TestAdd()
    {
        AddItemToDesiredSlot(_testItem, 10);
    }
}
