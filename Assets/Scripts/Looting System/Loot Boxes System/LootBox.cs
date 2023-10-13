using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LootBox : MonoBehaviour
{
    [SerializeField] private LootBoxGeneratingSet _set;
    [SerializeField] private bool _demolishing;
    [SerializeField] private List<InventoryCell> _cells;

    private void Start()
    {
        gameObject.tag = "LootBox";
        GenerateCells();
    }

    private void Update()
    {
        CheckCells();
    }

    private void GenerateCells()
    {
        foreach (var item in _set.Items)
            _cells.Add(new InventoryCell(item.Item, Random.Range(item.MinimalCount, item.MaximalCount)));
    }

    public void Open(InventoryHandler handler)
    {
        handler.OpenLootBoxPanel();
        handler.LootBoxSlotsContainer.InitCells(_cells);
    }

    private void CheckCells()
    {
        if(!_demolishing) return;
        foreach (var cell in _cells)
            if(cell.Item != null) return;
        Destroy(gameObject);
    }

    // public void RemoveCell(Item item, int count)
    // {
    //     foreach (var cell in Cells)
    //     {
    //         if(cell.Item == null || item == null) return;
    //         if (cell.Item.Id == item.Id && cell.Count == count)
    //         {
    //             Cells.Remove(cell);
    //             CheckCells();
    //             return;
    //         }
    //     }
    // }
}