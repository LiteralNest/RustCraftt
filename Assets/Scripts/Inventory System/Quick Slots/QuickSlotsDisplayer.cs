using System.Collections.Generic;
using UnityEngine;

public class QuickSlotsDisplayer : MonoBehaviour
{
    [SerializeField] private List<QuickSlotDisplayer> _quickSlots = new List<QuickSlotDisplayer>();

    private void Start()
        => ClearSlots();
    
    private void ClearSlots()
    {
        foreach (var slot in _quickSlots)
            slot.ClearSlot();
    }

    private ItemDisplayer GetGeneratedItemDisplayer(QuickSlotDisplayer quickSlotDisplayer, SlotDisplayer slotDisplayer)
    {
        var slotTransform = quickSlotDisplayer.transform;
        var instance = Instantiate(InventorySlotDisplayerSelector.singleton.GetDisplayerByType(slotDisplayer.ItemDisplayer.InventoryCell.Item), slotTransform.position, slotTransform.rotation, slotTransform);
        instance.SetInventoryCell(slotDisplayer.ItemDisplayer.InventoryCell);
        // instance.SetNewCell(slotDisplayer);
        return instance;
    }

    public void AssignSlots(List<SlotDisplayer> slotDisplayers)
    {
        ClearSlots();
        for (int i = 0; i < slotDisplayers.Count; i++)
        {
            if(slotDisplayers[i].ItemDisplayer == null) continue;
            var ItemDisplayer = GetGeneratedItemDisplayer(_quickSlots[i], slotDisplayers[i]);
            _quickSlots[i].AssignItemDisplayer(ItemDisplayer);
        }
    }
}