using System.Collections.Generic;
using UnityEngine;

public class StorageSlotsContainer : SlotsContainer
{
    private Storage _targetStorage;

    private Storage _bufferedStorage;
    
    public void InitCells(List<InventoryCell> cells, Storage storage)
    {
        _targetStorage = storage;
        Appear();
        Cells = cells;
        _bufferedStorage = storage;
        SlotsDisplayer.DisplayCells();
        _targetStorage = storage;
       
    }

    public override void ResetCell(int index)
    {
        base.ResetCell(index);
        _targetStorage.ResetItemServerRpc(index);
    }

    public override void AddCell(int index, InventoryCell cell)
    {
        base.AddCell(index, cell);
        _bufferedStorage.SetItemServerRpc(index, cell.Item.Id, cell.Count);
    }
}