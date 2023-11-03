using UnityEngine;

public class QuickSlotDisplayer : MonoBehaviour
{
    [SerializeField] private InventoryHandler _inventoryHandler;

    [Header("UI")] [SerializeField] private GameObject _activeFon;

    public ItemDisplayer ItemDisplayer { get; private set; }

    public void ClearSlot()
    {
        if (!ItemDisplayer) return;
        Destroy(ItemDisplayer.gameObject);
        ItemDisplayer = null;
    }

    public void AssignItemDisplayer(ItemDisplayer itemDisplayer)
    {
        ItemDisplayer = itemDisplayer;
        Destroy(itemDisplayer.GetComponent<DoubleTapHandler>());
    }

    public void Click()
    {
        if (GlobalValues.CanDragInventoryItems) return;
        if (ItemDisplayer == null)
        {
            _inventoryHandler.InHandObjectsContainer.SetDefaultHands();
            return;
        }
        if (ItemDisplayer.InventoryCell == null) return;
        var cell = ItemDisplayer.InventoryCell;
        if (cell == null) return;
        var item = cell.Item;
        item.Click(this, _inventoryHandler, out bool shouldMinus);
    }
}