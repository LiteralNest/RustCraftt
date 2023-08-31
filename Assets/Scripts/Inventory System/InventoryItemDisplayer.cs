using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemDisplayer : MonoBehaviour, IBeginDragHandler ,IDragHandler, IEndDragHandler
{
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _itemIcon;
    
    private InventorySlotDisplayer _slot;
    public InventoryCell InventoryCell { get; private set; }
    private SlotsContainer _slotsContainer;
    public void Init(InventorySlotDisplayer slot, SlotsContainer slotsContainer)
    {
        if (_slot != null)
            _slot.ClearItem();
        _slotsContainer = slotsContainer;
        Transform cellTransform = slot.transform;
        transform.position = cellTransform.position;
        transform.SetParent(cellTransform);
        _slot = slot;
    }

    public void Init(InventorySlotDisplayer slot, InventoryCell cell, SlotsContainer slotsContainer)
    {
        Init(slot, slotsContainer);
        InventoryCell = new InventoryCell(cell);
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

    private void SetInventoryCellData(InventoryCell cell)
        => _slot.Inventory.SetItemAt(_slot.Index, cell);
    
    private void DestroyCell()
    {
        _slot.ClearItem();
        Destroy(gameObject);
    }
    
    private void DisplayData()
    {
        if(InventoryCell.Item == null) return;
        _itemIcon.sprite = InventoryCell.Item.Icon;
        _countText.text = InventoryCell.Count.ToString();
    }

    public void SetCount(int value)
    {
        InventoryCell.Count = value;
        DisplayData();
        SetInventoryCellData(InventoryCell);
    }
    
    public void MinusCount(int value)
    {
        InventoryCell.Count -= value;
        if (InventoryCell.Count <= 0)
        {
            DestroyCell();
            return;
        }
        DisplayData();
        SetInventoryCellData(InventoryCell);
    }
    
    public void AddCount(int value)
    {
        InventoryCell.Count += value;
        DisplayData();
        SetInventoryCellData(InventoryCell);
    }
}
