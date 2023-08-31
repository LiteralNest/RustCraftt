using System.Collections.Generic;
using UnityEngine;

public class CraftingQueue : MonoBehaviour
{
    [Header("UI")] [SerializeField] private CreatingQueueAlertDisplayer _alertDisplayer;
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

    public void DisplayAlert(bool value)
    {
        if (_alertDisplayer.gameObject.activeSelf != value)
            _alertDisplayer.gameObject.SetActive(value);
    }
    
    public void DisplayAlert(Item item, int count, int time)
    {
        DisplayAlert(true);
        _alertDisplayer.Init(item, count, time);
    }
}