using System.Collections.Generic;
using UnityEngine;

public class StorageSlotsContainer : SlotsContainer
{
    [Tooltip("Leave blank if you don't need a filter")] [SerializeField]
    private List<InventoryCell> _avaliableCells = new List<InventoryCell>();

    private Storage _targetStorage;

    public override bool CanAddItem(Item item)
    {
        if (_avaliableCells.Count == 0) return true;
        foreach (var cell in _avaliableCells)
            if (cell.Item.Id == item.Id)
                return true;
        return false;
    }

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