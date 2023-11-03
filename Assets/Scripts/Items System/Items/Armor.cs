using UnityEngine;

[CreateAssetMenu(menuName = "Item/Armor")]
public class Armor : CraftingItem
{
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
    {
        base.Click(slotDisplayer, handler, out shouldMinus);
        shouldMinus = false;
    }
}