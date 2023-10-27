using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class SlotsContainer : MonoBehaviour
{
    [field: SerializeField] public SlotsDisplayer SlotsDisplayer { get; private set; }
    [field: SerializeField] public List<InventoryCell> Cells { get; protected set; }

    #region virtual

    public virtual void AddCell(int index, InventoryCell cell)
    {
        Cells[index].Count = cell.Count;
        Cells[index].Item = cell.Item;
    }

    public virtual bool CanAddItem(Item item)
        => true;

    #endregion

    public void ResetCell(int index)
    {
        Cells[index].Item = null;
        Cells[index].Count = 0;
    }

    public void AddItemToDesiredSlot(Item item, int count)
    {
        var slot = InventoryHelper.GetDesiredCell(item, count, Cells);
        if (slot == null) return;
        slot.Item = item;
        slot.Count += count;
        GlobalEventsContainer.InventoryItemAdded?.Invoke(new InventoryCell(item, count));
        GlobalEventsContainer.InventoryDataShouldBeSaved?.Invoke(Cells);
        SlotsDisplayer.DisplayCells();
    }

    public void RemoveItemFromDesiredSlot(Item item, int count)
    {
        InventoryHelper.RemoveItem(item, count, Cells);
        GlobalEventsContainer.InventoryItemRemoved?.Invoke(new InventoryCell(item, count));
        GlobalEventsContainer.InventoryDataShouldBeSaved?.Invoke(Cells);
        SlotsDisplayer.DisplayCells();
    }

    public int GetItemCount(Item item)
        => InventoryHelper.GetItemCount(item, Cells);

    public bool EnoughMaterials(List<InventoryCell> inputSlots)
    {
        List<InventoryCell> slots = new List<InventoryCell>(inputSlots);
        List<InventoryCell> cells = new List<InventoryCell>(Cells);
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (slot.Item == null) continue;
            for (int j = 0; j < cells.Count; j++)
            {
                var cell = cells[j];
                if (cell.Item == null) continue;
                if (cell.Item.Id == slot.Item.Id && cell.Count >= slot.Count)
                {
                    InventoryHelper.RemoveItem(cell.Item, cell.Count, slots);
                    if (!cells.Contains(cell)) i--;
                }
                   
            }
        }
        if (slots.Count == 0) return true;
        return false;
    }

    public void AssignCells(List<InventorySendingDataField> dataCells)
    {
        for (int i = 0; i < dataCells.Count; i++)
        {
            Cells[i].Item = ItemsContainer.singleton.GetItemById(dataCells[i].ItemId);
            Cells[i].Count = dataCells[i].Count;
        }
        SlotsDisplayer.DisplayCells();
    }
}