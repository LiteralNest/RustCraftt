using UnityEngine;

namespace Inventory_System.Slots_Displayer
{
    public class StorageSlotsDisplayer : SlotsDisplayer
    {
        [SerializeField] private bool _canSetSlots;
    
        public override void InitItems()
        {
            foreach (var cell in CellDisplayers)
                cell.CanSetSlot = _canSetSlots;
        }
    }
}