public class GatheringOre : Ore
{
    private void Start()
        => gameObject.tag = "Gathering";

    public async void Gather()
    {
        if (Recovering) return;
        // InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(_targetResource, 1);
        Recovering = true;
        await Destroy();
        Recovering = false;
    }
}