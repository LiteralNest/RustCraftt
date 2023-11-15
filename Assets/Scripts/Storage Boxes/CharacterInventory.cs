public class CharacterInventory : Storage
{
    protected override void Appear()
    {
      
    }
    
    public override void Open(InventoryHandler handler)
    {
        SlotsDisplayer = handler.InventorySlotsDisplayer;
        base.Open(handler);
    }
}
