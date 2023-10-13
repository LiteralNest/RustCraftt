using System.Collections.Generic;

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
}