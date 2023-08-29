using UnityEngine;
using TMPro;

public class AddingItemAlertDisplayer : AlertDisplayer
{
    [SerializeField] private TMP_Text _itemTitle;
    [SerializeField] private TMP_Text _itemCount;

    public void Init(InventoryCell inventoryCell)
    {
        _itemTitle.text = inventoryCell.Item.Name;
        _itemCount.text = "+" + inventoryCell.Count.ToString();
    }
}