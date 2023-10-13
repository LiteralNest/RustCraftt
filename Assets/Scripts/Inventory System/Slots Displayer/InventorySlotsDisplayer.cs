using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsDisplayer : SlotsDisplayer
{
    [Header("Attached Scripts")] [SerializeField]
    private QuickSlotsDisplayer _quickSlotsDisplayer;

    public override void InitItems()
    {
        foreach (var cell in _cellDisplayers)
            cell.CanSetSlot = true;
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