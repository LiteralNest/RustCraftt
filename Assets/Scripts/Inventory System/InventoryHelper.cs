using System.Collections.Generic;
using Unity.VisualScripting;

public static class InventoryHelper
{
    private static InventoryCell GetFreeCell(List<InventoryCell> cells)
    {
        foreach (var cell in cells)
            if (cell.Item == null)
                return cell;
        return null;
    }
    
    public static InventoryCell GetDesiredCell(int itemId, int count, List<InventoryCell> cells)
    {
        foreach (var cell in cells)
        {
            if(cell.Item == null) continue;
            if (cell.Item.Id == itemId)
            {
                if (cell.Item.StackCount >= count + cell.Count)
                    return cell;
            }
        }
        return GetFreeCell(cells); 
    }

    public static void AddItemToDesiredSlot(int itemId, int count, List<InventoryCell> cells)
    {
        var cell = GetDesiredCell(itemId, count, cells);
        cell.Item = ItemFinder.singleton.GetItemById(itemId);
        cell.Count += count;
    }
    
    private static int GetFreeCellId(List<InventoryCell> cells)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].Item == null)
                return i;
            
        }
        return -1;
    }
    
    public static int GetDesiredCellId(int itemId, List<InventoryCell> cells)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].Item == null) continue;
            if(cells[i].Item.Id == itemId)
                return i;
        }
        return GetFreeCellId(cells);
    }

    public static bool EnoughMaterials(List<InventoryCell> inputSlots, List<InventoryCell> targetSlots)
    {
        List<InventoryCell> slots = new List<InventoryCell>(inputSlots);
        List<InventoryCell> cells = new List<InventoryCell>(targetSlots);
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (slot.Item == null) continue;
            for (int j = 0; j < cells.Count; j++)
            {
                var cell = cells[j];
                if (cell.Item == null) continue;
                if (cell.Item.Id == slot.Item.Id && cell.Count >= slot.Count)
                {
                    slots.Remove(slot);
                    i--;
                    if (slots.Count == 0) return true;
                }
            }
        }
        if (slots.Count == 0) return true;
        return false;
    }
    
    public static void RemoveItem(int itemId, int count, List<InventoryCell> cells)
    {
        int currentCount = count;
        for (int i = 0; i < cells.Count; i++)
        {
            var cell = cells[i];
            if (cell.Item.Id == itemId)
            {
                if (currentCount >= cell.Count)
                {
                    currentCount -= cell.Count;
                    cell.Item = null;
                    cell.Count = 0;
                    i--;
                    if (currentCount <= 0)
                        return;
                    continue;
                }
                cell.Count -= currentCount;
                return;
            }
        }
    }
    
    public static int GetItemCount(int itemId, List<InventoryCell> cells)
    {
        int res = 0;
        foreach (var cell in cells)
        {
            if(cell.Item == null) continue;
            if(cell.Item.Id == itemId)
                res += cell.Count;
        }
        return res;
    }
}