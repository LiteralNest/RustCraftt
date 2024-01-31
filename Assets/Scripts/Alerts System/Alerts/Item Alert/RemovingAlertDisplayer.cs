using Alerts_System.Alerts.Item_Alert;

public class RemovingAlertDisplayer : ItemAlertDisplayer
{
    public override void Init(InventoryCell inventoryCell)
    {
        base.Init(inventoryCell);
        _itemCount.text = "-" + inventoryCell.Count;
    }
}
