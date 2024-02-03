namespace AlertsSystem.AlertTypes.Item_Alert
{
    public class AddingItemAlertDisplayer : ItemAlertDisplayer
    {
        public override void Init(string itemName, int count)
        {
            base.Init(itemName, count);
            _itemCount.text = "+" + count;
        }
    }
}