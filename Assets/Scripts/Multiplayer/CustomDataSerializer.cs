using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class CustomDataSerializer
{
    private static void ClearList(NetworkList<Vector2Int> res)
        => res.Clear();
    
    public static void SetConvertedItemsList(List<InventoryCell> cells, NetworkList<Vector2Int> res)
    {
        ClearList(res);
        foreach (var cell in cells)
        {
            if (cell.Item == null)
                res.Add(new Vector2Int(-1, 0));
            else
                res.Add(new Vector2Int(cell.Item.Id, cell.Count));
        }
    }

    public static List<InventoryCell> GetConvertedCellsList(NetworkList<Vector2Int> data)
    {
        List<InventoryCell> res = new List<InventoryCell>();

        foreach (var cell in data)
        {
            if (cell.x == -1)
                res.Add(new InventoryCell(null, 0));
            else
                res.Add(new InventoryCell(ItemsContainer.singleton.GetItemById(cell.x), cell.y));
        }
        
        return res;
    }
}