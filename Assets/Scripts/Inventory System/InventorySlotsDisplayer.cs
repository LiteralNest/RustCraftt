using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsDisplayer : MonoBehaviour
{
    [Header("Attached Scripts")] [SerializeField]
    private QuickSlotsDisplayer _quickSlotsDisplayer;

    [SerializeField] private SlotsContainer _slotsContainer;

    [Header("Start Init")] [SerializeField]
    private InventoryItemDisplayer _itemDisplayerPrefab;

    [SerializeField] private List<InventorySlotDisplayer> _cellDisplayers = new List<InventorySlotDisplayer>();
    
    private void Start()
    {
        if (_slotsContainer == null)
            _slotsContainer = GetComponent<SlotsContainer>();
        InitCells();
        DisplayCells();
    }

    private void InitCells()
    {
        for (int i = 0; i < _cellDisplayers.Count; i++)
            _cellDisplayers[i].Init(i, this, _slotsContainer);
    }

    private ItemDisplayer GetGeneratedItemDisplayer(InventoryCell cell, InventorySlotDisplayer slotDisplayer)
    {
        var itemDisplayer = Instantiate(_itemDisplayerPrefab, slotDisplayer.transform);
        itemDisplayer.Init(slotDisplayer, cell, _slotsContainer);
        return itemDisplayer;
    }

    public void DisplayCells()
    {
        if(!_slotsContainer) return;
        var cells = _slotsContainer.Cells;
        for (int i = 0; i < cells.Count; i++)
        {
            if (!cells[i].Item) continue;
            _cellDisplayers[i].SetItem(GetGeneratedItemDisplayer(cells[i], _cellDisplayers[i]));
        }
    }

    public List<SlotDisplayer> GetQuickSlots()
    {
        List<SlotDisplayer> res = new List<SlotDisplayer>();
        foreach (var slotDisplayer in _cellDisplayers)
        {
            if (slotDisplayer.IsQuickSlot)
                res.Add(slotDisplayer);
        }

        return res;
    }

    public void DisplayQuickSlots()
        => _quickSlotsDisplayer.AssignSlots(GetQuickSlots());

    public void ResetCells()
    {
        foreach (var cell in _cellDisplayers)
            cell.DestroyItem();
    }
}