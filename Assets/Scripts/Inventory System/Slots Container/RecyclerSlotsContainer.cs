using System.Collections.Generic;
using UnityEngine;

public class RecyclerSlotsContainer : SlotsContainer
{
    [SerializeField] private SlotsDisplayer _slotsDisplayer;
    private Recycler _recycler;
    public void InitCells(List<InventoryCell> cells)
    {
        Cells = cells;
        _slotsDisplayer.DisplayCells();
    }

    public void Init(Recycler recycler)
        => _recycler = recycler;
    
    public void Turn(bool value)
    {
        _recycler.SetTurned(value);
    }
}