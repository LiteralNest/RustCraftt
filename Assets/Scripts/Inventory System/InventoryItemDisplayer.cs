using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemDisplayer : MonoBehaviour, IBeginDragHandler ,IDragHandler, IEndDragHandler
{
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _itemIcon;
    
    private InventorySlotDisplayer _slot;
    public InventoryItem InventoryItem { get; private set; }

    public void Init(InventorySlotDisplayer slot)
    {
        if (_slot != null)
            _slot.ClearItem();

        Transform cellTransform = slot.transform;
        transform.position = cellTransform.position;
        transform.SetParent(cellTransform);
        _slot = slot;
    }

    public void Init(InventorySlotDisplayer slot, InventoryItem item)
    {
        Init(slot);
        InventoryItem = new InventoryItem(item);
        DisplayData();
    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!GlobalValues.CanDragInventoryItems) return;
        _slot.ClearItem();
        _countText.gameObject.SetActive(false);
        transform.SetParent(transform.root);
        _itemIcon.raycastTarget = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if(!GlobalValues.CanDragInventoryItems) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!GlobalValues.CanDragInventoryItems) return;
        _slot.Init(this);
        _countText.gameObject.SetActive(true);
        transform.position = _slot.transform.position;
        transform.SetParent(_slot.transform);
        _itemIcon.raycastTarget = true;
    }

    private void SetInventoryCellData(InventoryItem item)
        => _slot.Inventory.SetItemAt(_slot.Index, item);
    
    private void DestroyCell()
    {
        _slot.ClearItem();
        Destroy(gameObject);
    }
    
    private void DisplayData()
    {
        _itemIcon.sprite = InventoryItem.Item.Icon;
        _countText.text = InventoryItem.Count.ToString();
    }

    public void SetCount(int value)
    {
        InventoryItem.Count = value;
        DisplayData();
        SetInventoryCellData(InventoryItem);
    }
    
    public void MinusCount(int value)
    {
        InventoryItem.Count -= value;
        if (InventoryItem.Count <= 0)
        {
            DestroyCell();
            return;
        }
        DisplayData();
        SetInventoryCellData(InventoryItem);
    }
    
    public void AddCount(int value)
    {
        InventoryItem.Count += value;
        DisplayData();
        SetInventoryCellData(InventoryItem);
    }
}
