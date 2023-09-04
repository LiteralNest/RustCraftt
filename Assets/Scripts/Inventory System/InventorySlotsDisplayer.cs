using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsDisplayer : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private SlotsContainer _slotsContainer;

    [Header("Start Init")] 
    [SerializeField] private InventoryItemDisplayer _itemDisplayerPrefab;
    [SerializeField] private List<InventorySlotDisplayer> _cellDisplayers = new List<InventorySlotDisplayer>();

    private void Start()
    {
        if (_slotsContainer == null)
            _slotsContainer = GetComponent<SlotsContainer>();
        InitCells();
    }

    private void InitCells()
    {
        for (int i = 0; i < _cellDisplayers.Count; i++)
        {
            _cellDisplayers[i].Index = i;
            _cellDisplayers[i].Init(_slotsContainer);
            
        }
    }

    private void ClearPlace(Transform place)
    {
        foreach (Transform child in place)
        {
            if(!child.TryGetComponent(out InventoryItemDisplayer itemDisplayer)) continue;
            Destroy(child.gameObject);
        }
    }
    
    private void CreateCell(InventoryCell cell, int index)
    {
        InventorySlotDisplayer slotDisplayer = _cellDisplayers[index];
        ClearPlace(slotDisplayer.transform);
        var instance = Instantiate(_itemDisplayerPrefab, slotDisplayer.transform);
        instance.Init(slotDisplayer, cell, _slotsContainer);
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

    public void DeleteCellAt(int index)
    {
        if (index >= _cellDisplayers.Count)
        {
            Debug.LogError("Index out of range " + index);
            return;
        }

        _cellDisplayers[index].DeleteItem();
    }

    public void ResetCells()
    {
        foreach (var cellDisplayer in _cellDisplayers)
        {
            if(cellDisplayer.transform.childCount == 0) continue;
            Destroy(cellDisplayer.transform.GetChild(0).gameObject);
        }
    }
}
