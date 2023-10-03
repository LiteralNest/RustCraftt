using System.Collections.Generic;

public class CampFireSlotsContainer : SlotsContainer
{
    private CampFireHandler _campFireHandler;
    
    public override void ResetItemAt(int index)
    {
        _campFireHandler.RemoveCell(new InventoryCell(Cells[index].Item, Cells[index].Count));
        base.ResetItemAt(index);
    }

    public override void SetItemAt(int index, InventoryCell cell)
    {
        _campFireHandler.AddCell(cell);
        base.SetItemAt(index, cell);
    }
    
    public void Init(CampFireHandler handler, List<InventoryCell> cells)
    {
        _campFireHandler = handler;
        _cellsDisplayer.ResetCells();
        ResetCells();
        foreach (var cell in cells)
            AddItemToDesiredSlot(cell.Item, cell.Count);
    }

    public void SetFlaming(bool value)
        => _campFireHandler.TurnFlamingServerRpc(value);
}
