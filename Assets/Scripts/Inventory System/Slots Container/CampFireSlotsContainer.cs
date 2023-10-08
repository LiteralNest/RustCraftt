using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CampFireSlotsContainer : SlotsContainer
{
    [field:SerializeField] public InventorySlotsDisplayer SlotsDisplayer { get; private set; }
    [SerializeField] private CampFireDisplayer _campFireDisplayer;
    public CampFireHandler CampFireHandler { get; private set; }

    public void Init(CampFireHandler campFireHandler)
    {
        CampFireHandler = campFireHandler;
        SlotsDisplayer.ResetCells();
        Cells = new List<InventoryCell>(campFireHandler.Cells);
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
        if (ListContains(item, CampFireHandler.AvaliableFuel) ||
            ListContains(item, CampFireHandler.AvaliableFoodForCooking)) return true;
        return false;
    }

    public override void AddCell(int index, InventoryCell cell)
    {
        base.AddCell(index, cell);
        CampFireHandler.SetItem(index, cell);
    }

    public void TurnFire(bool value)
    {
        if(!CampFireHandler) return;
        CampFireHandler.TurnFlamingServerRpc(value);
    }

}