using System.Collections.Generic;
using System.Threading.Tasks;
using Armor_System.UI;
using UnityEngine;

public class InventorySlotsDisplayer : SlotsDisplayer
{
    [Header("Attached Scripts")] [SerializeField]
    private QuickSlotsDisplayer _quickSlotsDisplayer;

    [SerializeField] private int _mainSlotsCount;
    [SerializeField] private int _armorCellsCount;

    public override List<ArmorSlotDisplayer> GetArmorSlots()
    {
        var res = new List<ArmorSlotDisplayer>();
        for (int i = _mainSlotsCount; i < CellDisplayers.Count; i++)
            res.Add(CellDisplayers[i] as ArmorSlotDisplayer);
        return res;
    }

    public override void InitItems()
    {
        foreach (var cell in CellDisplayers)
        {
            cell.CanSetSlot = true;
        }
    }

    public List<SlotDisplayer> GetQuickSlots()
    {
        List<SlotDisplayer> res = new List<SlotDisplayer>();
        foreach (var slotDisplayer in CellDisplayers)
        {
            if (slotDisplayer.IsQuickSlot)
                res.Add(slotDisplayer);
        }

        return res;
    }

    public void DisplayQuickSlots()
        => _quickSlotsDisplayer.AssignSlots(GetQuickSlots());

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