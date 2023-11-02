[System.Serializable]
public class InventoryCell
{
   public Item Item;
   public int Count;

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