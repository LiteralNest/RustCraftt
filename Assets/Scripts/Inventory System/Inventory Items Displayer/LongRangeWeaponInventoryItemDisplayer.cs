public class LongRangeWeaponInventoryItemDisplayer : ItemDisplayer
{
    public override void DisplayData()
    {
        if (InventoryCell.Item == null) return;
        _itemIcon.sprite = InventoryCell.Item.Icon;
        DisplayAmmoCount(0);
    }

    public void DisplayAmmoCount(int ammo)
        => _countText.text = ammo.ToString();
}