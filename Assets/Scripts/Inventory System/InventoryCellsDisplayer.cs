using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(InventoryCellsDisplayer))]
public class InventoryCellsDisplayer : MonoBehaviour
{
    [FormerlySerializedAs("_cellsContainer")]
    [Header("Attached Scripts")]
    [SerializeField] private InventorySlotsContainer slotsContainer;

    [Header("Start Init")] [SerializeField]
    private InventoryItemDisplayer _itemDisplayerPrefab;
    [SerializeField] private List<InventorySlotDisplayer> _cellDisplayers = new List<InventorySlotDisplayer>();
    
    private void Start()
    {
        if (slotsContainer == null)
            slotsContainer = GetComponent<InventorySlotsContainer>();
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
        foreach (var item in slotsContainer.Cells)
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

        CreateCell(slotsContainer.Cells[index], index);
    }
}
