using System.Collections.Generic;
using AlertsSystem.AlertTypes.Alerts;
using Items_System.Items.Abstract;
using UnityEngine;

public class CraftingQueue : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform _placeForCells;
    [SerializeField] private CraftingQueueSlotFunctional _slotPrefab;


    private List<CraftingQueueSlotFunctional> _slots = new List<CraftingQueueSlotFunctional>();

    private void CheckForCreatingCells()
    {
        foreach (var cell in _slots)
            if (cell.Creating)
                return;
        if (_slots.Count <= 0) return;
        _slots[0].CreateItems();
    }

    public void CreateCell(CraftingItem item, int count, List<CraftingItemDataTableSlot> slots)
    {
        var instance = Instantiate(_slotPrefab, transform);
        instance.Init(item, count, this, slots, _placeForCells);
        _slots.Add(instance);
        CheckForCreatingCells();
    }

    public void DeleteCell(CraftingQueueSlotFunctional slot)
    {
        _slots.Remove(slot);
        CheckForCreatingCells();
    }
}