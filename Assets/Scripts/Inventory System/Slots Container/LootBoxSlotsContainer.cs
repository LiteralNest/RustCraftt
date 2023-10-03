using System.Collections.Generic;

public class LootBoxSlotsContainer : SlotsContainer
{
    private LootBox _targetBox;

    public override void ResetItemAt(int index)
    {
        _targetBox.RemoveCell(Cells[index].Item, Cells[index].Count);
        base.ResetItemAt(index);
    }

    public void Init(LootBox lootBox, List<InventoryCell> cells)
    {
        _cellsDisplayer.ResetCells();
        ResetCells();
        foreach (var cell in cells)
            AddItemToDesiredSlot(cell.Item, cell.Count);
        _targetBox = lootBox;
    }
}