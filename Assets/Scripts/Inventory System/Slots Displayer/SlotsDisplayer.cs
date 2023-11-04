using System.Collections.Generic;
using ArmorSystem.UI;
using UnityEngine;

public abstract class SlotsDisplayer : MonoBehaviour
{
    [SerializeField] protected SlotsContainer _slotsContainer;

    [Header("Start Init")] [SerializeField]
    protected InventoryItemDisplayer _itemDisplayerPrefab;

    [SerializeField] protected List<InventorySlotDisplayer> _cellDisplayers = new List<InventorySlotDisplayer>();
    
    public abstract void InitItems();

    public virtual List<ArmorSlotDisplayer> GetArmorSlots()
        => null;

    private void Start()
    {
        if (_slotsContainer == null)
            _slotsContainer = GetComponent<SlotsContainer>();
        InitItems();
        InitCells();
        DisplayCells();
    }
    
    private void InitCells()
    {
        for (int i = 0; i < _cellDisplayers.Count; i++)
            _cellDisplayers[i].Init(i, this, _slotsContainer);
    }

    public void ResetCells()
    {
        foreach (var cell in _cellDisplayers)
            cell.DestroyItem();
    }

    private ItemDisplayer GetGeneratedItemDisplayer(InventoryCell cell, InventorySlotDisplayer slotDisplayer)
    {
        var itemDisplayer = Instantiate(_itemDisplayerPrefab, slotDisplayer.transform);
        itemDisplayer.Init(slotDisplayer, cell, _slotsContainer);
        return itemDisplayer;
    }


    public virtual void DisplayCells()
    {
        ResetCells();
        var cells = _slotsContainer.Cells;
        for (int i = 0; i < cells.Count; i++)
        {
            if (!cells[i].Item) continue;
            _cellDisplayers[i].SetItem(GetGeneratedItemDisplayer(cells[i], _cellDisplayers[i]));
        }
    }

    [ContextMenu("Save Data")]
    public void SaveData()
        => GlobalEventsContainer.InventoryDataShouldBeSaved?.Invoke(_slotsContainer.Cells);
}