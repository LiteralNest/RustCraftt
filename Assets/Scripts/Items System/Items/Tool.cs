using UnityEngine;

[CreateAssetMenu(menuName = "Item/Tool")]
public class Tool : CraftingItem
{
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
    {
        shouldMinus = false;
        handler.SetActiveItem(this);
        GlobalEventsContainer.ShouldActivateSlot?.Invoke(slotDisplayer.Index);
    }
}