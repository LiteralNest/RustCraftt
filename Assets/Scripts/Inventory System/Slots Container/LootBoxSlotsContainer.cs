using System.Collections.Generic;
using UnityEngine;

public class LootBoxSlotsContainer : SlotsContainer
{
    [SerializeField] private LootBoxSlotsDisplayer _lootBoxSlotsDisplayer;
    private LootBox _targetLootBox;
    public void InitCells(List<InventoryCell> cells, LootBox lootBox)
    {
        Appear();
        Cells = cells;
        _lootBoxSlotsDisplayer.DisplayCells();
        _targetLootBox = lootBox;
    }

    public override void ResetCell(int index)
    {
        base.ResetCell(index);
        _targetLootBox.AssignCellsAndSendData(Cells);
    }
}