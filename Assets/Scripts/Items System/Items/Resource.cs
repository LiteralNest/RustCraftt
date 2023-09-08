using UnityEngine;

[CreateAssetMenu(menuName = "Item/Resource")]
public class Resource : Item
{
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
    {
        base.Click(slotDisplayer, handler, out shouldMinus);
        shouldMinus = false;
    }
}