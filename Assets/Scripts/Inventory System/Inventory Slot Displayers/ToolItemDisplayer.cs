namespace Inventory_System.Inventory_Slot_Displayers
{
    public class ToolItemDisplayer : DamagableItemDisplayer
    {
        private int _hp;
        private int _cachedHp;

        public override void DisplayData()
        {
            base.DisplayData();
            if(InventoryCell.Item == null) return;
            _itemIcon.sprite = InventoryCell.Item.Icon;
            //Hp bar
        }
    }
}
