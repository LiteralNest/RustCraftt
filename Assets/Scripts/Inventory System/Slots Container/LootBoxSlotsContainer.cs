using System.Collections.Generic;
using UnityEngine;

public class LootBoxSlotsContainer : SlotsContainer
{
    [SerializeField] private LootBoxSlotsDisplayer _lootBoxSlotsDisplayer;

    public void InitCells(List<InventoryCell> cells)
    {
        Cells = cells;
        _lootBoxSlotsDisplayer.DisplayCells();
    }
}