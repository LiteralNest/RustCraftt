using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StorageSlotsContainer : SlotsContainer
{
    [FormerlySerializedAs("_lootBoxSlotsDisplayer")] [SerializeField] private StorageSlotsDisplayer storageSlotsDisplayer;
    private Storage _targetStorage;

    protected override void SendDataToServer()
    {
        _targetStorage.AssignCellsAndSendData(Cells);
    }
    
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
        _targetStorage.AssignCellsAndSendData(Cells);
    }
}