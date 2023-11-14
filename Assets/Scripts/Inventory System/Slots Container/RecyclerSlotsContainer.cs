using System.Collections.Generic;
using UnityEngine;

public class RecyclerSlotsContainer : SlotsContainer
{
    [Header("UI")] 
    [SerializeField] private GameObject _activeButton;
    [SerializeField] private GameObject _unactiveButton;
    
    private Recycler _recycler;
    
    public override void AddCell(int index, InventoryCell cell)
    {
        base.AddCell(index, cell);
        _recycler.SetItemServerRpc(index, cell.Item.Id, cell.Count);
    }
    
    public void InitCells(List<InventoryCell> cells)
    {
        Cells = cells;
        SlotsDisplayer.DisplayCells();
    }

    public void Init(Recycler recycler)
    {
        // Appear();
        _recycler = recycler;
    }

    public void Turn(bool value)
    {
        _recycler.SetTurned(value);
    }

    public void DisplayButtons(bool value)
    {
        if (value)
        {
            _activeButton.SetActive(false);
            _unactiveButton.SetActive(true);
            return;
        }
        _activeButton.SetActive(true);
        _unactiveButton.SetActive(false);
    }
}