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
    
    public static InventoryCell GetDesiredCell(Item item, int count, List<InventoryCell> cells)
    {
        foreach (var cell in cells)
        {
            if (cell.Item == item)
            {
                if (cell.Item.StackCount >= count + cell.Count)
                    return cell;
            }
        }
        return GetFreeCell(cells); 
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
    
    public static int GetDesiredCellId(Item item, int count, List<InventoryCell> cells)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if(cells[i].Item == item)
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
    
    public static void RemoveItem(Item item, int count, List<InventoryCell> cells)
    {
        int currentCount = count;
        for (int i = 0; i < cells.Count; i++)
        {
            var cell = cells[i];
            if (cell.Item == item)
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
    
    public static int GetItemCount(Item item, List<InventoryCell> cells)
    {
        int res = 0;
        foreach (var cell in cells)
        {
            if(cell.Item == null) continue;
            if(cell.Item.Id == item.Id)
                res += cell.Count;
        }
        return res;
    }
}