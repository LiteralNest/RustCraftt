using System.Collections.Generic;

public class StorageSlotsContainer : SlotsContainer
{
    private Storage _targetStorage;

    public void InitCells(List<InventoryCell> cells, Storage storage)
    {
        _targetStorage = storage;
        Appear();
        Cells = cells;
        SlotsDisplayer.DisplayCells();
    }

    public override void ResetCell(int index)
    {
        base.ResetCell(index);
        _targetStorage.ResetItemServerRpc(index);
    }

    public override void AddCell(int index, InventoryCell cell)
    {
        var obj = transform;
        _targetStorage.SetItemServerRpc(index, cell.Item.Id, cell.Count);
        base.AddCell(index, cell);
       
    }
}