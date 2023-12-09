using Storage_System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SlotDisplayer : MonoBehaviour, IDropHandler
{
    public ItemDisplayer ItemDisplayer { get; protected set; }
    [field: SerializeField] public bool IsQuickSlot { get; private set; }
    [field: SerializeField] public bool CanSetSlot { get; set; }

    public int Index { get; private set; }

    public SlotsDisplayer InventorySlotsDisplayer { get; protected set; }
    public Storage Inventory { get; protected set; }

    protected abstract void Drop(PointerEventData eventData);

    public void OnDrop(PointerEventData eventData)
        => Drop(eventData);

    public void Init(int index, SlotsDisplayer slotsDisplayer, Storage targetStorage)
    {
        Index = index;
        InventorySlotsDisplayer = slotsDisplayer;
        Inventory = targetStorage;
        ItemDisplayer = null;
    }

    public void DisplayItem(ItemDisplayer itemDisplayer)
    {
        if (ItemDisplayer != null) Destroy(ItemDisplayer.gameObject);
        ItemDisplayer = itemDisplayer;
        ItemDisplayer.SetNewCell(this);
    }
    
    private void SetItem(ItemDisplayer itemDisplayer)
    {
        if (ItemDisplayer != null) Destroy(ItemDisplayer.gameObject);
        itemDisplayer.PreviousCell.Inventory.ResetItemServerRpc(itemDisplayer.PreviousCell.Index);
        ItemDisplayer = itemDisplayer;
        ItemDisplayer.SetNewCell(this);
        var cell = ItemDisplayer.InventoryCell;
        Inventory.SetItemServerRpc(Index, new CustomSendingInventoryDataCell(cell.Item.Id, cell.Count, cell.Hp, cell.Ammo));
    }

    private void ResetItem()
    {
        ItemDisplayer = null;
    }

    public virtual void ResetItemWhileDrag()
        => ItemDisplayer = null;

    private void ClearPlace(Transform place)
    {
        foreach (Transform child in place)
            Destroy(child.gameObject);
    }

    public void DestroyItem()
    {
        if (transform.childCount != 0)
            ClearPlace(transform);
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
        var res = ItemDisplayer.StackCount(cell);
        if (res > 0) return false;
        DestroyItem();
        return true;
    }

    public virtual void Swap(ItemDisplayer itemDisplayer)
    {
        var prevCell = itemDisplayer.PreviousCell;
        prevCell.SetItem(ItemDisplayer);
        SetItem(itemDisplayer);
    }

    protected virtual bool TrySetItem(ItemDisplayer itemDisplayer)
    {
        if (!CanSetSlot) return false;
        Debug.Log(gameObject.name);
        if (!Inventory.CanAddItem(itemDisplayer.InventoryCell.Item, Index)) return false;
        if (CheckForFree(itemDisplayer)) return true;
        if (TryStack(itemDisplayer.InventoryCell, out bool wasStacking)) return true;
        if (wasStacking) return false;
        Swap(itemDisplayer);
        return true;
    }
}