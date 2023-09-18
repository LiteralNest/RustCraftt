using System.Collections.Generic;
using UnityEngine;

public class ToolCupboard : MonoBehaviour
{
    [SerializeField] private List<InventoryCell> _cells = new List<InventoryCell>();

    private bool TryRemoveItem(InventoryCell minusingCell)
    {
        var item = minusingCell.Item;
        int currentCount = minusingCell.Count;
        foreach (var cell in _cells)
        {
            if (cell.Item.Id == item.Id && cell.Count > currentCount)
            {
                if (currentCount > cell.Count)
                {
                    currentCount -= cell.Count;
                    cell.Count = 0;
                    if (currentCount <= 0)
                        return true;
                    continue;
                }
                cell.Count -= currentCount;
                return true;
            }
        }

        return false;
    }
    
    public bool TryRemoveItems(List<InventoryCell> cells)
    {
        foreach (var cell in cells)
        {
            if (!TryRemoveItem(cell)) return false;
        }
        return true;
    }
}
