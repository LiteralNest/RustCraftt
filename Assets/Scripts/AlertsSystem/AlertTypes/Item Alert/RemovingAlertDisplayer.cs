namespace AlertsSystem.AlertTypes.Item_Alert
{
    public class RemovingAlertDisplayer : ItemAlertDisplayer
    {
        public override void Init(string itemName, int itemCount)
        {
            base.Init(itemName, itemCount);
            _itemCount.text = "-" + itemCount;
        }
    }
}
