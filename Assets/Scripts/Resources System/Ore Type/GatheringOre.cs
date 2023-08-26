public class GatheringOre : Ore
{
    private bool _recovering;
    
    public async void Gather(InventorySlotsContainer inventory)
    {
        if(_recovering) return;
        inventory.AddItemToDesiredSlot(_targetResource, 1);
        _recovering = true;
        await Destroy();
        _recovering = false;
    }
}