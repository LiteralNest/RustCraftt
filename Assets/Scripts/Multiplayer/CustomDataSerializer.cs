using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class CustomDataSerializer
{
    public static void SetConvertedItemsList(List<InventoryCell> cells, NetworkList<Vector2> res)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            var cell = cells[i];
            if (cell.Item == null)
                res.Insert(0, new Vector2Int(cell.Item.Id, cell.Count));
            else
                res.Insert(0,new Vector2Int(cell.Item.Id, cell.Count));
        }
    }

    public static List<InventoryCell> GetConvertedCellsList(NetworkList<Vector2> data)
    {
        List<InventoryCell> res = new List<InventoryCell>();

        foreach (var cell in data)
        {
            if (cell.x == -1)
                res.Add(new InventoryCell(null, 0));
            else
                res.Add(new InventoryCell(ItemsContainer.singleton.GetItemById((int)cell.x), (int)cell.y));
        }
        
        return res;
    }
}