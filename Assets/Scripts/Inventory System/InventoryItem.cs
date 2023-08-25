using UnityEngine;

[System.Serializable]
public class InventoryItem
{
   [field:SerializeField] public Item Item { get; set; }
   [field:SerializeField] public int Count { get; set; }

   public InventoryItem(InventoryItem item)
   {
      Item = item.Item;
      Count = item.Count;
   }

   public InventoryItem(Item item, int count)
   {
      Item = item;
      Count = count;
   }
}