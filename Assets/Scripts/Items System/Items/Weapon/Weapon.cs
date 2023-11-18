public class Weapon : Tool
{
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler)
    {
        base.Click(slotDisplayer, handler);
        MainUiHandler.singleton.ActivateAttackButton(true);
    }
}