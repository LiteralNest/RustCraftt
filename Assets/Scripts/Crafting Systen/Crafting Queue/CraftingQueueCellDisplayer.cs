using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingQueueCellDisplayer : MonoBehaviour, IPointerDownHandler
{
    [Header("UI")] [SerializeField] 
    private TMP_Text _timeText;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _displayingIcon;

    [SerializeField] private List<CraftingItemDataTableSlot> _reservedSlotsForOneItem;

    public bool Creating { get; private set; }
    
    private CraftingQueue _queue;
    
    private bool _stopCreating = false;
    private CraftingItem _craftingItem;
    private int _count;

    public void Init(CraftingItem craftingItem, int count, CraftingQueue queue, List<CraftingItemDataTableSlot> slots)
    {
        _craftingItem = craftingItem;
        _count = count;
        _queue = queue;
        _reservedSlotsForOneItem = slots;
        DisplayCountText();
        DeleteItemsFromInventory();
    }

    private void DeleteItemsFromInventory()
    {
        foreach (var slot in _reservedSlotsForOneItem)
            InventorySlotsContainer.singleton.DeleteSlot(slot.Resource, slot.Count * _count);
    }
    
    private void ReturnItemsToInventory()
    {
        foreach (var slot in _reservedSlotsForOneItem)
            InventorySlotsContainer.singleton.AddItemToDesiredSlot(slot.Resource, slot.Count * _count);
    }
    
    private void DisplayTimeText(int time)
    {
        _timeText.text = time + "s";
    }
    
    private void DisplayCountText()
    {
        _countText.text = _count.ToString();
    }
    

    private IEnumerator CreateItemsRoutine()
    {
        Creating = true;
        while (_count > 0)
        {
            for (int i = _craftingItem.TimeForCreating; i > 0; i--)
            {
                DisplayTimeText(i);
                yield return new WaitForSeconds(1);
                _queue.DisplayAlert(_craftingItem, _count, i);
            }
            InventorySlotsContainer.singleton.AddItemToDesiredSlot(_craftingItem,1);
            _count--;
            DisplayCountText();
        }
        Delete();
    }

    public void CreateItems()
        => StartCoroutine(CreateItemsRoutine());

    private void Delete(bool shouldRecoverData = false)
    {
        _stopCreating = true;
        _queue.DisplayAlert(false);
        _queue.DeleteCell(this);
        if (shouldRecoverData)
            ReturnItemsToInventory();
        Destroy(gameObject);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Delete(true);
    }
}