using System.Collections.Generic;
using System.Threading.Tasks;
using ArmorSystem.UI;
using UnityEngine;

public class InventorySlotsDisplayer : SlotsDisplayer
{
    [Header("Attached Scripts")] [SerializeField]
    private QuickSlotsDisplayer _quickSlotsDisplayer;

    [SerializeField] private int _mainSlotsCount;
    [SerializeField] private int _armorCellsCount;

    private void OnEnable()
        => GlobalEventsContainer.ShouldDisplayInventoryCells += DisplayCells;

    private void OnDisable()
        => GlobalEventsContainer.ShouldDisplayInventoryCells -= DisplayCells;

    public override List<ArmorSlotDisplayer> GetArmorSlots()
    {
        var res = new List<ArmorSlotDisplayer>();
        for (int i = _mainSlotsCount; i < _cellDisplayers.Count; i++)
            res.Add(_cellDisplayers[i] as ArmorSlotDisplayer);
        return res;
    }

    public override void InitItems()
    {
        foreach (var cell in _cellDisplayers)
        {
            cell.CanSetSlot = true;
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

    private async void DisplayQuickCells()
    {
        await Task.Delay(100);
        DisplayQuickSlots();
    }

    public override void DisplayCells()
    {
        base.DisplayCells();
        DisplayQuickCells();
    }
}