using UnityEngine;

public class LootBoxSlotsDisplayer : SlotsDisplayer
{
    public override void InitItems()
    {
        foreach (var cell in _cellDisplayers)
            cell.CanSetSlot = false;
    }
}