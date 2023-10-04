using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SlotDisplayer : MonoBehaviour, IDropHandler
{
    public ItemDisplayer ItemDisplayer { get; protected set; }
    [field:SerializeField] public bool IsQuickSlot { get; private set; }
    
    public int Index { get; private set; }
    
    public InventorySlotsDisplayer InventorySlotsDisplayer { get; protected set; }
    public SlotsContainer Inventory { get; protected set; }

    protected abstract void Drop(PointerEventData eventData);
    
    public void OnDrop(PointerEventData eventData)
        => Drop(eventData);

    public void Init(int index, InventorySlotsDisplayer slotsDisplayer, SlotsContainer slotsContainer)
    {
        Index = index;
        InventorySlotsDisplayer = slotsDisplayer;
        Inventory = slotsContainer;
        ItemDisplayer = null;
    }
    
    public void SetItem(ItemDisplayer itemDisplayer)
    {
        if(ItemDisplayer != null) Destroy(ItemDisplayer.gameObject);
        ItemDisplayer = itemDisplayer;
        ItemDisplayer.SetNewCell(this);
    }

    public void ResetItem()
    {
        ItemDisplayer = null;
    }
    
    public void DestroyItem()
    {
        if(!ItemDisplayer) return;
        Destroy(ItemDisplayer.gameObject);
        ResetItem();
    }

    private bool CheckForFree(ItemDisplayer itemDisplayer)
    {
        if (ItemDisplayer) return false;
        SetItem(itemDisplayer);
        return true;
    }
    
    private bool TryStack(InventoryCell cell, out bool wasStacking)
    {
        wasStacking = false;
        if (cell.Item == null || cell.Item != ItemDisplayer.InventoryCell.Item) return false;
        wasStacking = true;
        var res = ItemDisplayer.StackCount(cell.Count);
        if (res > 0) return false;
        return true;
    }

    private void Swap(ItemDisplayer itemDisplayer)
    {
        var prevCell = itemDisplayer.PreviousCell;
        prevCell.SetItem(ItemDisplayer);
        SetItem(itemDisplayer);
    }
    
    protected bool TrySetItem(ItemDisplayer itemDisplayer)
    {
        if (!Inventory.CanAddItem(itemDisplayer.InventoryCell.Item)) return false;
        if (CheckForFree(itemDisplayer)) return true;
        if(TryStack(itemDisplayer.InventoryCell, out bool wasStacking)) return true;
        if(wasStacking) return false;
        Swap(itemDisplayer);
        return true;
    }
}