using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlotDisplayer : InventorySlotDisplayer, IPointerClickHandler
{
    [SerializeField] private InventoryHandler _inventoryHandler;
    
    [Header("UI")]
    [SerializeField] private GameObject _activeFon;

    private void OnEnable()
        => GlobalEventsContainer.ShouldActivateSlot += SetActiveDisplaying;
    
    private void OnDisable()
        => GlobalEventsContainer.ShouldActivateSlot -= SetActiveDisplaying;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(GlobalValues.CanDragInventoryItems) return;
        if (ItemDisplayer == null || ItemDisplayer.InventoryCell == null) return;
        var cell = ItemDisplayer.InventoryCell;
        if(cell == null) return;
        var item = cell.Item;
        if(item == null) return;
        item.Click(this, _inventoryHandler, out bool shouldMinus);
        if(!shouldMinus) return;
        Inventory.RemoveItemCountAt(ItemDisplayer.InventoryCell.Item,1, Index);
    }

    private void SetActiveDisplaying(int index)
    {
        if (Index == index)
        {
            GlobalEventsContainer.ShouldDisplayHandItem?.Invoke(ItemDisplayer.InventoryCell.Item.Id, _inventoryHandler.PlayerNetCode.GetClientId());
            _activeFon.SetActive(true);
            return;
        }
        _activeFon.SetActive(false);
    }
}
