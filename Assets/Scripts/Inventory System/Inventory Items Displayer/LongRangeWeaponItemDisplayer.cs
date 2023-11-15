namespace Inventory_System.Inventory_Items_Displayer
{
    public class LongRangeWeaponItemDisplayer : InventoryItemDisplayer
    {
        public override void DisplayData()
        {
            if (InventoryCell.Item == null) return;
            _itemIcon.sprite = InventoryCell.Item.Icon;
            _countText.text = "0";
        }
    }
}
