using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ItemDisplayer : MonoBehaviour
{
    [SerializeField] protected TMP_Text _countText;
    [SerializeField] protected Image _itemIcon;
    public InventoryCell InventoryCell { get; protected set; }
    public SlotDisplayer PreviousCell { get; protected set; }
    
    public void SetInventoryCell(InventoryCell inventoryCell)
    {
        InventoryCell = inventoryCell;
        DisplayData();
    }
    
    public void DisplayData()
    {
        if(InventoryCell.Item == null) return;
        _itemIcon.sprite = InventoryCell.Item.Icon;
        _countText.text = InventoryCell.Count.ToString();
    }

    public int StackCount(int addedCount)
    {
        var currentItemCount = InventoryCell.Count;
        int count = currentItemCount + addedCount;
        if (count <= InventoryCell.Item.StackCount)
        {
            InventoryCell.Count = count;
            return 0;
        }

        InventoryCell.Count = InventoryCell.Item.StackCount;
        return count - InventoryCell.Item.StackCount;
    }
    
    public void SetPosition()
        => transform.position = PreviousCell.transform.position;
    
    public virtual void SetNewCell(SlotDisplayer slotDisplayer)
    {
        PreviousCell = slotDisplayer;
        var slotTransform = slotDisplayer.transform;
        transform.SetParent(slotTransform);
        SetPosition();
    }
}