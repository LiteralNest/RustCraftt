using TMPro;
using UnityEngine;

public class CraftingItemDataTableSlotDisplayer : MonoBehaviour
{
    [Header("Colors")] [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _unAvailableColor = Color.red;
    [Header("UI")] 
    [SerializeField] private TMP_Text _neededAmountText;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _selectedAmountText;
    [SerializeField] private TMP_Text _inventoryAmountText;

    [field:SerializeField] public bool ResourceAvailable { get; private set; }
    private CraftingItemDataTableSlot _slot;
    private int _inventoryCount;

    public void Init()
    {
        _neededAmountText.text = "";
        _titleText.text = "";
        _selectedAmountText.text = "";
        _inventoryAmountText.text = "";
        ResourceAvailable = true;
    }

    public void Init(CraftingItemDataTableSlot slot)
    {
        _slot = slot;
        _neededAmountText.text = slot.Count.ToString();
        _titleText.text = slot.Resource.Name;
        CalculateAmount(1);
    }

    private void DisplayInventoryCount(int count)
    {
        if(_slot.Resource == null) return;
        var inventoryCount = InventorySlotsContainer.singleton.GetItemCount(_slot.Resource);
        _inventoryCount = inventoryCount;
        int globalCount = count * _slot.Count;
        ResourceAvailable  = _inventoryCount >= globalCount;
        DisplayInventoryText(_inventoryCount, ResourceAvailable);
    }

    private void DisplayInventoryText(int count, bool avaliable)
    {
        Color color = _defaultColor;
        if(!avaliable)
            color = _unAvailableColor;
        _inventoryAmountText.color = color;
        _inventoryAmountText.text = count.ToString();
    }
    
    public void CalculateAmount(int count)
    {
        DisplayInventoryCount(count);
        _selectedAmountText.text = (count * _slot.Count).ToString();
    }
}