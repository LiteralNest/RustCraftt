using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryCellsDisplayer))]
public class InventoryCellsDisplayer : MonoBehaviour
{
    public static InventoryCellsDisplayer singleton;

    [Header("UI")] 
    [SerializeField] private GameObject _mainButtonsPanel;
    [SerializeField] private GameObject _armorPanel;
    [SerializeField] private GameObject _lootBoxPanel;
    [SerializeField] private GameObject _inventoryPanel;

    [Header("Attached Scripts")]
    [SerializeField] private InventorySlotsContainer _slotsContainer;

    [Header("Start Init")] 
    [SerializeField] private InventoryItemDisplayer _itemDisplayerPrefab;
    [SerializeField] private List<InventorySlotDisplayer> _cellDisplayers = new List<InventorySlotDisplayer>();

    private void Awake()
        => singleton = this;
    
    private void Start()
    {
        if (_slotsContainer == null)
            _slotsContainer = GetComponent<InventorySlotsContainer>();
    }

    private void CreateCell(InventoryCell cell, int index)
    {
        InventorySlotDisplayer slotDisplayer = _cellDisplayers[index];
        if(slotDisplayer.transform.childCount > 0)
            Destroy(slotDisplayer.transform.GetChild(0).gameObject);
        var instance = Instantiate(_itemDisplayerPrefab, slotDisplayer.transform);
        instance.Init(slotDisplayer, cell);
        slotDisplayer.Init(instance);
    }
    
    [ContextMenu("Display Inventory")]
    public void DisplayCells()
    {
        int counter = 0;
        foreach (var item in _slotsContainer.Cells)
        {
            counter++;
            _cellDisplayers[counter - 1].Index = counter - 1;
            if(item.Item == null) continue;
            CreateCell(item, counter - 1);
        }
    }

    public void DisplayCellAt(int index)
    {
        if (index >= _cellDisplayers.Count)
        {
            Debug.LogError("Index out of range " + index);
            return;
        }

        CreateCell(_slotsContainer.Cells[index], index);
    }

    public void HandleInventory(bool isOpen)
    {
        _mainButtonsPanel.SetActive(!isOpen);
        _inventoryPanel.SetActive(isOpen);
        GlobalValues.CanDragInventoryItems = isOpen;
        GlobalValues.CanLookAround = !isOpen;
    }

    public void OpenArmorPanel()
    {
        HandleInventory(true);
        _armorPanel.SetActive(true);
        _lootBoxPanel.SetActive(false);
    }

    public void OpenLootBoxPanel()
    {
        HandleInventory(true);
        _armorPanel.SetActive(false);
        _lootBoxPanel.SetActive(true);
    }
}
