using System.Collections.Generic;
using Inventory_System;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer
{
    public static class CustomDataSerializer
    {
        public static void SetConvertedItemsList(List<InventoryCell> cells, NetworkList<Vector3> res)
        {
            res.Clear();
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (cell.Item != null)
                    res.Add(new Vector3Int(cell.Item.Id, cell.Count, cell.Hp));
                else
                    res.Add(new Vector3Int(-1, 0, -1));
            }
        }

        public static List<InventoryCell> GetConvertedCellsList(NetworkList<Vector3> data)
        {
            List<InventoryCell> res = new List<InventoryCell>();

            foreach (var cell in data)
            {
                if (cell.x == -1)
                    res.Add(new InventoryCell(null, 0, -1));
                else
                    res.Add(new InventoryCell(ItemFinder.singleton.GetItemById((int)cell.x), (int)cell.y, (int)cell.z));
            }
        
            return res;
        }
    }
}