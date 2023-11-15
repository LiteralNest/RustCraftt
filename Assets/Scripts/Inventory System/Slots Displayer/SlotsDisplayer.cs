using System.Collections.Generic;
using ArmorSystem.UI;
using Storage_Boxes;
using UnityEngine;

public abstract class SlotsDisplayer : MonoBehaviour
{
    public Storage TargetStorage { get; set; }

    [Header("Start Init")]
    [SerializeField] protected List<InventorySlotDisplayer> _cellDisplayers = new List<InventorySlotDisplayer>();
    
    public abstract void InitItems();

    public virtual List<ArmorSlotDisplayer> GetArmorSlots()
        => null;

    private void Start()
    {
        InitItems();
    }
    
    public void InitCells()
    {
        for (int i = 0; i < _cellDisplayers.Count; i++)
            _cellDisplayers[i].Init(i, this, TargetStorage);
    }

    public void ResetCells()
    {
        foreach (var cell in _cellDisplayers)
            cell.DestroyItem();
    }

    private ItemDisplayer GetGeneratedItemDisplayer(InventoryCell cell, InventorySlotDisplayer slotDisplayer)
    {
        var itemDisplayer = Instantiate(InventorySlotDisplayerSelector.singleton.GetDisplayerByType(cell.Item), slotDisplayer.transform);
        itemDisplayer.Init(slotDisplayer, cell, TargetStorage);
        return itemDisplayer;
    }
    
    public virtual void DisplayCells()
    {
        ResetCells();
        var cells = TargetStorage.Cells;
        for (int i = 0; i < cells.Count; i++)
        {
            if (!cells[i].Item) continue;
            _cellDisplayers[i].SetItem(GetGeneratedItemDisplayer(cells[i], _cellDisplayers[i]), false);
        }
    }

    public void AssignStorage(Storage storage)
        => TargetStorage = storage;
}