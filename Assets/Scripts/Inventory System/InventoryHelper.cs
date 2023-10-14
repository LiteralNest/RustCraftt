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

    public static void RemoveItem(Item item, int count, List<InventoryCell> cells)
    {
        int currentCount = count;
        foreach (var cell in cells)
        {
            if (cell.Item == item)
            {
                if (currentCount > cell.Count)
                {
                    currentCount -= cell.Count;
                    cell.Count = 0;
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