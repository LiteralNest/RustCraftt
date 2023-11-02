using System.Collections.Generic;
using UnityEngine;

public class StorageSlotsContainer : SlotsContainer
{
    [SerializeField] private StorageSlotsDisplayer storageSlotsDisplayer;
    private Storage _targetStorage;
    
    public void InitCells(List<InventoryCell> cells, Storage storage)
    {
        _targetStorage = storage;
        Appear();
        Cells = cells;
        storageSlotsDisplayer.DisplayCells();
    }

    public override void ResetCell(int index)
    {
        base.ResetCell(index);
        _targetStorage.ResetItemServerRpc(index);
    }

    public override void AddCell(int index, InventoryCell cell)
    {
        base.AddCell(index, cell);
        _targetStorage.SetItemServerRpc(index, cell.Item.Id, cell.Count);
    }
}