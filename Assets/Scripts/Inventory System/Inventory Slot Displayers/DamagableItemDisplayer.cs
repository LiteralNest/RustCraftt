using Inventory_System.Inventory_Items_Displayer;

namespace Inventory_System.Inventory_Slot_Displayers
{
    public class DamagableItemDisplayer : InventoryItemDisplayer
    {
        private int _hp;
        private int _cachedHp;

        public override void DisplayData()
        {
            if(InventoryCell.Item == null) return;
            _itemIcon.sprite = InventoryCell.Item.Icon;
            //Hp bar
        }
    }
}
