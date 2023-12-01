using Items_System.Items.Abstract;

[System.Serializable]
public class InventoryCell
{
   public Item Item;
   public int Count;
   public int Hp = -1;

   public InventoryCell(InventoryCell cell)
   {
      Item = cell.Item;
      Count = cell.Count;
      Hp = cell.Hp;
   }

   public InventoryCell(Item item, int count, int hp = -1)
   {
      Item = item;
      Count = count;
      Hp = hp;
   }
}