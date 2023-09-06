using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventorySlotsDisplayer))]
public abstract class SlotsContainer : MonoBehaviour
{
    [SerializeField] protected InventorySlotsDisplayer _cellsDisplayer;
    [field: SerializeField] public List<InventoryCell> Cells { get; private set; }

    private void Start()
    {
        if (_cellsDisplayer == null)
            _cellsDisplayer = GetComponent<InventorySlotsDisplayer>();
    }
    
    #region virtual

    public virtual void ResetItemAt(int index)
    {
        if (index >= Cells.Count)
        {
            Debug.LogError("Index out of range " + index);
            return;
        }

        Cells[index].Item = null;
        Cells[index].Count = 0;
    }
    
    public virtual void AddItemToDesiredSlot(Item item, int count)
    {
        int resCount = TrySetItemToSimilar(item, count);
        if(resCount > 0)
        {
            int cellIndex = GetFreeItemCellIndex();
            Cells[cellIndex] = new InventoryCell(item, resCount);
            _cellsDisplayer.DisplayCellAt(cellIndex);
        }
    }
    
    public virtual void DeleteSlot(Item item, int count)
    {
        int currentCount = count;

        for (int i = 0; i < Cells.Count; i++)
        {
            var cell = Cells[i];
            if(cell.Item == null) continue;
            if (cell.Item.Id == item.Id)
            {
                GlobalEventsContainer.InventoryItemRemoved?.Invoke(new InventoryCell(item, count));
                currentCount = GetMinusItemFromCellWithDifference(i, currentCount, cell);
            }
            if(currentCount == 0)
                break;
        }
    }

    #endregion

    protected void ResetCells()
    {
        foreach (var cell in Cells)
        {
            cell.Count = 0;
            cell.Item = null;
        }
    }

    public void SetItemAt(int index, InventoryCell cell)
    {
        if (index >= Cells.Count)
        {
            Debug.LogError("Index out of range " + index);
            return;
        }

        Cells[index] = new InventoryCell(cell);
    }

    public virtual void RemoveItemCountAt(Item item, int count, int index)
    {
        if (index >= Cells.Count)
        {
            Debug.LogError("Index out of range " + index);
            return;
        }
         var cell =  Cells[index];
         cell.Count -= count;
         _cellsDisplayer.DisplayCellAt(index);
         if(cell.Count > 0) return;
         _cellsDisplayer.DeleteCellAt(index);
    }
    
    private void SetItemCount(int index, int count)
    {
        Cells[index].Count = count;
        _cellsDisplayer.DisplayCellAt(index);
    }
    
    private int GetFreeItemCellIndex()
    {
        int index = 0;
        foreach (var cell in Cells)
        {
            if (cell.Item == null) return index;
            index++;
        }
        return -1;
    }

    private int TrySetItemToSimilar(Item item, int count)
    {
        int currentCount = count;

        int index = -1;
        foreach (var cell in Cells)
        {
            index++;
            if(cell.Item == null || cell.Item.Id != item.Id) continue;
            int sum = currentCount + cell.Count;
            if (sum < cell.Item.StackCount)
            {
                SetItemCount(index, sum);
                return 0;
            }
            currentCount = sum - cell.Item.StackCount;
            SetItemCount(index, cell.Item.StackCount);
        }
        return currentCount;
    }
    
    public int GetItemCount(Item item)
    {
        int count = 0;
        foreach (var cell in Cells)
        {
            if (cell.Item == null) continue;
            if (cell.Item.Id == item.Id)
                count += cell.Count;
        }
        return count;
    }

    private int GetMinusItemFromCellWithDifference(int index, int count, InventoryCell cell)
    {
        if (count >= cell.Count)
        {
            _cellsDisplayer.DeleteCellAt(index);
            Cells.Remove(cell);
            return count - cell.Count;
        }

        cell.Count -= count;
        _cellsDisplayer.DisplayCellAt(index);
        return 0;
    }

    private bool TryDeleteCellFromList(List<InventoryCell> targetList, InventoryCell checkingCell)
    {
        int currentCount = checkingCell.Count;
        for (int i = 0; i < targetList.Count; i++)
        {
            if(targetList[i].Item == null) continue;
            if (targetList[i].Item.Id == checkingCell.Item.Id)
            {
                int res = targetList[i].Count - currentCount;
                if (targetList[i].Count < 0)
                {
                    currentCount = Mathf.Abs(res);
                    targetList.RemoveAt(i);
                    i--;
                    continue;
                }
                return true;
            }
        }
        if (currentCount <= 0) return true;
        return false;
    }
    
    public bool ItemsAvaliable(List<InventoryCell> cells)
    {
        List<InventoryCell> targetCells = new List<InventoryCell>(Cells);
        foreach (var cell in cells)
            if (!TryDeleteCellFromList(targetCells, cell)) return false;
        return true;
    }
}