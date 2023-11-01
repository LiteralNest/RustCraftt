using System.Threading.Tasks;

public class StorageBox : Storage
{
    public override void InitBox()
        => InitStorageBox();

    public override void Open(InventoryHandler handler)
        => OpenBox(handler);
    
    public override void CheckCells(){}
    private async void InitStorageBox()
        => BoxId.Value = await WebServerDataHandler.singleton.RegistrateNewStorageBox();
    
    private async void OpenBox(InventoryHandler handler)
    {
        await LoadCells();
        handler.LargeStorageSlotsContainer.InitCells(_cells, this);
        handler.OpenLargeChestPanel();
    }
    
    private async Task LoadCells()
    {
        var cells = await WebServerDataHandler.singleton.LoadStorageBoxData(BoxId.Value);
        AssignCells(cells);
    }
}