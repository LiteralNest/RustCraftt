using System.Collections.Generic;
using UnityEngine;

public abstract class SlotsContainer : MonoBehaviour
{
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
}