using System.Collections.Generic;
using UnityEngine;

public class CampFireSlotsContainer : SlotsContainer
{
    [SerializeField] private CampFireDisplayer _campFireDisplayer;
    public CampFireHandler CampFireHandler { get; private set; }

    public void Init(List<InventoryCell> cells, CampFireHandler campFireHandler)
    {
        Appear();
        CampFireHandler = campFireHandler;
        SlotsDisplayer.ResetCells();
        Cells = new List<InventoryCell>(cells);
        SlotsDisplayer.DisplayCells();
        _campFireDisplayer.DisplayButton(campFireHandler.Flaming.Value);
    }

    private bool ListContains(Item item, List<Fuel> list)
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
        if(item is Food || item is CookingFood) return true;
        return false;
    }

    public override void AddCell(int index, InventoryCell cell)
    {
        base.AddCell(index, cell);
        CampFireHandler.SetItemServerRpc(index, cell.Item.Id, cell.Count);
    }

    public void TurnFire(bool value)
    {
        if(!CampFireHandler) return;
        CampFireHandler.TurnFlamingServerRpc(value);
    }
}