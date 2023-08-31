using UnityEngine;
using TMPro;

public abstract class ItemAlertDisplayer : AlertDisplayer
{
    [SerializeField] protected TMP_Text _itemTitle;
    [SerializeField] protected TMP_Text _itemCount;

    public virtual void Init(InventoryCell inventoryCell)
    {
        if(inventoryCell.Item == null) return;
        _itemTitle.text = inventoryCell.Item.Name;
    }
}
