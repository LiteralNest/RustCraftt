public class Weapon : Tool
{
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
    {
        base.Click(slotDisplayer, handler, out shouldMinus);
        GlobalEventsContainer.AttackButtonActivated?.Invoke(true);
    }
}