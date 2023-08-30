public class GatheringOre : Ore
{
    private bool _recovering;
    
    public async void Gather()
    {
        if(_recovering) return;
        InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(_targetResource, 1);
        _recovering = true;
        await Destroy();
        _recovering = false;
    }
}