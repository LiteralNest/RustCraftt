using UnityEngine;

public class StorageSlotsDisplayer : SlotsDisplayer
{
    [SerializeField] private bool _canSetSlots;
    
    public override void InitItems()
    {
        foreach (var cell in _cellDisplayers)
            cell.CanSetSlot = _canSetSlots;
    }
}