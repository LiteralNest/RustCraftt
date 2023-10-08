using System.Collections.Generic;
using UnityEngine;

public class CampFireSlotsContainer : SlotsContainer
{
    [SerializeField] private InventorySlotsDisplayer _slotsDisplayer;
    private CampFireHandler _campFireHandler;

    public void Init(CampFireHandler campFireHandler)
    {
        _campFireHandler = campFireHandler;
        _slotsDisplayer.ResetCells();
        Cells = new List<InventoryCell>(campFireHandler.Cells);
        _slotsDisplayer.DisplayCells();
    }

    private bool ListContains(Item item, List<Item> list)
    {
        foreach (var cell in list)
            if (item.Id == cell.Id)
                return true;
        return false;
    }

    private bool ListContains(Item item, List<CookingFood> list)
    {
        foreach (var cell in list)
            if (item.Id == cell.Id)
                return true;
        return false;
    }

    public override bool CanAddItem(Item item)
    {
        if (ListContains(item, _campFireHandler.AvaliableFuel) ||
            ListContains(item, _campFireHandler.AvaliableFoodForCooking)) return true;
        return false;
    }

    public override void AddCell(int index, InventoryCell cell)
    {
        base.AddCell(index, cell);
        _campFireHandler.SetItem(index, cell);
    }

    public void TurnFire(bool value)
    {
        if(!_campFireHandler) return;
        _campFireHandler.TurnFlamingServerRpc(value);
    }

}