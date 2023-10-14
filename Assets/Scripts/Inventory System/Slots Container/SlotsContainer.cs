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
        SlotsDisplayer.DisplayCells();
    }

    public void RemoveItemFromDesiredSlot(Item item, int count)
    {
        InventoryHelper.RemoveItem(item, count, Cells);
        GlobalEventsContainer.InventoryItemRemoved?.Invoke(new InventoryCell(item, count));
        SlotsDisplayer.DisplayCells();
    }

    public int GetItemCount(Item item)
        => InventoryHelper.GetItemCount(item, Cells);
}