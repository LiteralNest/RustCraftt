using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingQueueSlotFunctional : MonoBehaviour
{
    [SerializeField] private CraftingQueueSlotDisplayer _craftingQueueSlotDisplayerPref;
    private List<CraftingItemDataTableSlot> _reservedSlotsForOneItem = new List<CraftingItemDataTableSlot>();
    
    private CraftingQueueSlotDisplayer _currentSlotDisplayer;
    
    public bool Creating { get; private set; }
    private CraftingQueue _queue;
    
    private CraftingItem _craftingItem;
    private int _count;

    public void Init(CraftingItem craftingItem, int count, CraftingQueue queue, List<CraftingItemDataTableSlot> slots, Transform placeForCreatingCell)
    {
        _reservedSlotsForOneItem = slots;
        _craftingItem = craftingItem;
        _count = count;
        _queue = queue;
        CreateDisplayer(craftingItem, count, placeForCreatingCell);
        DeleteItemsFromInventory();
    }

    private void CreateDisplayer(CraftingItem craftingItem, int count, Transform placeForCreatingCell)
    {
        _currentSlotDisplayer = Instantiate(_craftingQueueSlotDisplayerPref, placeForCreatingCell);
        _currentSlotDisplayer.Init(craftingItem, count, this);
    }
    
    private void DeleteItemsFromInventory()
    {
        // foreach (var slot in _reservedSlotsForOneItem)
        //     InventoryHandler.singleton.InventorySlotsContainer.DeleteSlot(slot.Resource, slot.Count * _count);
    }

    private void ReturnItemsToInventory()
    {
        // foreach (var slot in _reservedSlotsForOneItem)
        //     InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(slot.Resource, slot.Count * _count);
    }
    
    public void CreateItems()
        => StartCoroutine(CreateItemsRoutine());
    
    public void Delete(bool shouldRecoverData = false)
    {
        Destroy(_currentSlotDisplayer.gameObject); 
        _queue.DisplayAlert(false);
        _queue.DeleteCell(this);
        if (shouldRecoverData)
            ReturnItemsToInventory();
        Destroy(gameObject);
    }
    
    private IEnumerator CreateItemsRoutine()
    {
        Creating = true;
        while (_count > 0)
        {
            for (int i = _craftingItem.TimeForCreating; i > 0; i--)
            {
               _currentSlotDisplayer.DisplayTimeText(i);
                yield return new WaitForSeconds(1);
                _queue.DisplayAlert(_craftingItem, _count, i);
            }

            InventoryHandler.singleton.CharacterInventory.AddCraftedItem(_craftingItem.Id, 1);
            _count--;
            _currentSlotDisplayer.DisplayCountText(_count);
        }

        Delete();
    }
}
