public class RemovingAlertDisplayer : ItemAlertDisplayer
{
    public override void Init(InventoryCell inventoryCell)
    {
        base.Init(inventoryCell);
        _itemCount.text = "-" + inventoryCell.Count;
    }
}
