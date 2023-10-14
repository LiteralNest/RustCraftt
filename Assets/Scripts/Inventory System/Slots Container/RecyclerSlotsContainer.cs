using System.Collections.Generic;
using UnityEngine;

public class RecyclerSlotsContainer : SlotsContainer
{
    private Recycler _recycler;
    public void InitCells(List<InventoryCell> cells)
    {
        Cells = cells;
        SlotsDisplayer.DisplayCells();
    }

    public void Init(Recycler recycler)
        => _recycler = recycler;
    
    public void Turn(bool value)
    {
        _recycler.SetTurned(value);
    }
}