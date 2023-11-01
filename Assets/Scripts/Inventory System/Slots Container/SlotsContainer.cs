using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class SlotsContainer : MonoBehaviour
{
    [field: SerializeField] public SlotsDisplayer SlotsDisplayer { get; private set; }
    [field: SerializeField] public List<InventoryCell> Cells { get; protected set; }

    #region virtual

    protected virtual void SendDataToServer()
    {
        
    }

    protected virtual void Appear()
        => ActiveInvetoriesHandler.singleton.AddActiveInventory(this);
    
    public virtual void AddCell(int index, InventoryCell cell)
    {
        Cells[index].Count = cell.Count;
        Cells[index].Item = cell.Item;
        SendDataToServer();
    }

    public virtual bool CanAddItem(Item item)
        => true;

    #endregion

    public virtual void ResetCell(int index)
    {
        Cells[index].Item = null;
        Cells[index].Count = 0;
    }

    public virtual void ResetCellAndSendData(int index)
    {
        ResetCell(index);
        GlobalEventsContainer.InventoryDataShouldBeSaved?.Invoke(Cells);
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
        GlobalEventsContainer.InventoryDataChanged?.Invoke();
    }

    public void RemoveItemFromDesiredSlot(Item item, int count)
    {
        InventoryHelper.RemoveItem(item, count, Cells);
        GlobalEventsContainer.InventoryItemRemoved?.Invoke(new InventoryCell(item, count));
        GlobalEventsContainer.InventoryDataShouldBeSaved?.Invoke(Cells);
        SlotsDisplayer.DisplayCells();
        GlobalEventsContainer.InventoryDataChanged?.Invoke();
    }

    public int GetItemCount(Item item)
        => InventoryHelper.GetItemCount(item, Cells);

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