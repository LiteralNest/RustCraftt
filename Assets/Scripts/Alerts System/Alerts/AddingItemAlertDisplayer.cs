using UnityEngine;
using TMPro;

public class AddingItemAlertDisplayer : AlertDisplayer
{
    [SerializeField] private TMP_Text _itemTitle;
    [SerializeField] private TMP_Text _itemCount;

    public void Init(InventoryItem inventoryItem)
    {
        _itemTitle.text = inventoryItem.Item.Name;
        _itemCount.text = "+" + inventoryItem.Count.ToString();
    }
}