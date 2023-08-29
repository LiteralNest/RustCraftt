using UnityEngine;

[System.Serializable]
public class InventoryCell
{
   [field:SerializeField] public Item Item { get; set; }
   [field:SerializeField] public int Count { get; set; }

   public InventoryCell(InventoryCell cell)
   {
      Item = cell.Item;
      Count = cell.Count;
   }

   public InventoryCell(Item item, int count)
   {
      Item = item;
      Count = count;
   }
}