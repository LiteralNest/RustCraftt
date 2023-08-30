using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingQueueSlotCreator : MonoBehaviour
{
    [Header("Start Init")]
    [SerializeField] private int _currentCount = 1;
    
    [FormerlySerializedAs("_inventorySlots")] [Header("Attached Scripts")]
    [SerializeField] private CraftingQueue _craftingQueue;
    [SerializeField] private CraftingItemDataDisplayer _craftingItemDataDisplayer;
    [SerializeField] private CraftingItemDataTableSlotsContainer _craftingItemDataTableSlotsContainer;
    [SerializeField] private CraftingItemDataTableSlotsContainer _slotsContainer;
    [Header("UI")] [SerializeField] private TMP_Text _displayingCountText;

    private void OnEnable()
    {
        GlobalEventsContainer.InventoryDataChanged += ReCalculateResourceSlots;
    }
    
    private void OnDisable()
    {
        GlobalEventsContainer.InventoryDataChanged -= ReCalculateResourceSlots;
    }
    
    private void Start()
    {
        if(_craftingQueue == null)
            _craftingQueue = FindObjectOfType<CraftingQueue>();
        if(_craftingItemDataDisplayer == null)
            _craftingItemDataDisplayer = FindObjectOfType<CraftingItemDataDisplayer>();
        if(_craftingItemDataTableSlotsContainer == null)
            _craftingItemDataTableSlotsContainer = FindObjectOfType<CraftingItemDataTableSlotsContainer>();
        if (_slotsContainer == null)
            _slotsContainer = GetComponent<CraftingItemDataTableSlotsContainer>();
    }

    private void ReCalculateResourceSlots()
    {
        foreach (var slotDisplayer in _slotsContainer.SlotDisplayers)
            slotDisplayer.CalculateAmount(_currentCount);
    }

    private void DisplayCurrentCountText()
    =>  _displayingCountText.text = _currentCount.ToString();
    
    public void PlusCount()
    {
        _currentCount++;
        DisplayCurrentCountText();
        ReCalculateResourceSlots();
    }

    public void MinusCount()
    {
        _currentCount--;
        DisplayCurrentCountText();
        ReCalculateResourceSlots();
    }

    private bool CanBeCreated()
    {
        foreach(var slot in _slotsContainer.SlotDisplayers)
            if (!slot.ResourceAvailable)
                return false;
        return true;
    }
    
    public void CreateSlot()
    {
        if(!CanBeCreated()) return;
        List<CraftingItemDataTableSlot> slots = _craftingItemDataTableSlotsContainer.Slots;
        _craftingQueue.CreateCell(_craftingItemDataDisplayer.CurrentItem, _currentCount, slots);
        _currentCount = 1;
        DisplayCurrentCountText();
    }
}