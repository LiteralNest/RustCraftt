public class InventorySlotsContainer : SlotsContainer
{
    public static InventorySlotsContainer singleton { get; private set; }

    private void Awake()
        => singleton = this;

    private async void Start()
    {
        var cells = await WebServerDataHandler.singleton.LoadInventoryData();
        if(cells == null) return;
        AssignCells(cells);
    }
}