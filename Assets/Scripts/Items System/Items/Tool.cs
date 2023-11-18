using UnityEngine;

[CreateAssetMenu(menuName = "Item/Tool")]
public class Tool : DamagableItem
{
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler)
    {
        base.Click(slotDisplayer, handler);
        handler.SetActiveItem(this);
        GlobalEventsContainer.ShouldDisplayHandItem?.Invoke(slotDisplayer.ItemDisplayer.InventoryCell.Item.Id,
            handler.PlayerNetCode.GetClientId());
    }
}