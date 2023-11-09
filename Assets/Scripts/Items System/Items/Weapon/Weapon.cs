public class Weapon : Tool
{
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler)
    {
        base.Click(slotDisplayer, handler);
        GlobalEventsContainer.AttackButtonActivated?.Invoke(true);
    }
}