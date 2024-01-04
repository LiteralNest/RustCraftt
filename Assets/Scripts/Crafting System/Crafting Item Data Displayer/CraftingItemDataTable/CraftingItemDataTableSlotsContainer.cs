using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingItemDataTableSlotsContainer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CraftingItemDataTableSlotDisplayer _slotPref;
    [SerializeField] private Transform _placeForSlots;
    [SerializeField] private int _slotsCount = 3;
    
    [FormerlySerializedAs("_inventorySlotsContainer")]
    [Header("Attached Scripts")]
    [SerializeField] private CraftingItemDataTableSlotDisplayer _slotsDisplayer;

    public List<CraftingItemDataTableSlot> Slots { get; private set; }

    public List<CraftingItemDataTableSlotDisplayer> SlotDisplayers { get; private set; } =
        new List<CraftingItemDataTableSlotDisplayer>();

    private void Start()
    {
        if (_slotsDisplayer == null)
            _slotsDisplayer = FindObjectOfType<CraftingItemDataTableSlotDisplayer>();
    }

    private void ClearPlace(Transform place)
    {
        foreach (Transform child in place)
            Destroy(child.gameObject);
    }

    private void DisplaySlots()
    {
        ClearPlace(_placeForSlots);
        SlotDisplayers.Clear();
        int counter = 0;
        foreach (var slot in Slots)
        {
            var instance = Instantiate(_slotPref, _placeForSlots);
            instance.Init(slot);
            SlotDisplayers.Add(instance);
            counter++;
        }

        for (int i = counter; i < _slotsCount; i++)
        {
            Instantiate(_slotPref, _placeForSlots).Init();
        }
    }
    
    public void AddSlots(List<CraftingItemDataTableSlot> slots)
    {
        ClearPlace(_placeForSlots);
        Slots = new List<CraftingItemDataTableSlot>(slots);
        DisplaySlots();
    }
}