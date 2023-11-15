using Inventory_System.Inventory_Slot_Displayers;

namespace Inventory_System.Inventory_Items_Displayer
{
    public class LongRangeWeaponItemDisplayer : DamagableItemDisplayer
    {
        public override void DisplayData()
        {
            base.DisplayData();
            if (InventoryCell.Item == null) return;
            _itemIcon.sprite = InventoryCell.Item.Icon;
            _countText.text = "0";
        }
    }
}
