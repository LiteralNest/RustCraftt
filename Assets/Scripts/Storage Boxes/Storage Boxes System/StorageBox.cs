public class StorageBox : Storage
{
    public override void InitBox()
    {
        SaveNetData();
    }

    public override void Open(InventoryHandler handler)
        => OpenBox(handler);
    
    public override void CheckCells(){}

    private void OpenBox(InventoryHandler handler)
    {
        handler.LargeStorageSlotsContainer.InitCells(Cells, this);
        handler.OpenLargeChestPanel();
    }
}