namespace Storage_System
{
    public class BackPack : Storage
    {
        public override void Open(InventoryHandler handler)
        {
            SlotsDisplayer = handler.BackPackSlotsDisplayer;
            handler.InventoryPanelsDisplayer.OpenBackPackPanel();
            base.Open(handler);
        }
    }
}
