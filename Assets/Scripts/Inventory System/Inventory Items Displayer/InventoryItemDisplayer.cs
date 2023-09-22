public class InventoryItemDisplayer : ItemDisplayer
{
    public override void DisplayData()
    {
        if(InventoryCell.Item == null) return;
        _itemIcon.sprite = InventoryCell.Item.Icon;
        _countText.text = InventoryCell.Count.ToString();
    }
}
