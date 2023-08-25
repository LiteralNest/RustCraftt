using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryCellsDisplayer))]
public class InventorySlotsContainer : MonoBehaviour
{
    [SerializeField] private InventoryCellsDisplayer _cellsDisplayer;
    [field: SerializeField] public List<InventoryItem> Cells { get; private set; }
    [SerializeField] private Item _testItem;

    private void Start()
    {
        if (_cellsDisplayer == null)
            _cellsDisplayer = GetComponent<InventoryCellsDisplayer>();
    }

    public void ResetItemAt(int index)
    {
        if (index >= Cells.Count)
        {
            Debug.LogError("Index out of range " + index);
            return;
        }

        Cells[index].Item = null;
        Cells[index].Count = 0;
    }

    public void SetItemAt(int index, InventoryItem item)
    {
        if (index >= Cells.Count)
        {
            Debug.LogError("Index out of range " + index);
            return;
        }

        Cells[index] = new InventoryItem(item);
    }

    private void Awake()
        => gameObject.tag = "Inventory";

    private void SetItemCount(int index, int count)
    {
        Cells[index].Count = count;
        _cellsDisplayer.DisplayCellAt(index);
    }
    
    private int GetFreeItemCellIndex()
    {
        int index = 0;
        foreach (var cell in Cells)
        {
            if (cell.Item == null) return index;
            index++;
        }
        
        return -1;
    }

    private int TrySetItemToSimilar(Item item, int count)
    {
        int currentCount = count;

        int index = -1;
        foreach (var cell in Cells)
        {
            index++;
            if(cell.Item == null || cell.Item.ID != item.ID) continue;
            int sum = currentCount + cell.Count;
            if (sum < cell.Item.StackCount)
            {
                SetItemCount(index, sum);
                return 0;
            }
            currentCount = sum - cell.Item.StackCount;
            SetItemCount(index, cell.Item.StackCount);
        }
        return currentCount;
    }

    public void AddItemToDesiredSlot(Item item, int count)
    {
        GlobalEventsContainer.InventoryItemAdded?.Invoke(new InventoryItem(item, count));
        int resCount = TrySetItemToSimilar(item, count);
        if(resCount == 0) return;
        int cellIndex = GetFreeItemCellIndex();
        Cells[cellIndex] = new InventoryItem(item, resCount);
        _cellsDisplayer.DisplayCellAt(cellIndex);
    }
    
    [ContextMenu("Test Add Item")]
    private void TestAdd()
    {
        AddItemToDesiredSlot(_testItem, 10);
    }
}