public abstract class BuildingBluePrint : BluePrint
{
    private void OnEnable()
        => GlobalEventsContainer.InventoryDataChanged += CheckForAvailable;
    
    private void OnDisable()
        => GlobalEventsContainer.InventoryDataChanged -= CheckForAvailable;

    protected void Init()
        => CheckForAvailable();
}