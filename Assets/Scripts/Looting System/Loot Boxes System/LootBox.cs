using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    [SerializeField] private LootBoxGeneratingSet _set;
    [SerializeField] private bool _demolishing;
    [field: SerializeField] public List<InventoryCell> Cells { get; private set; }

    private void Start()
    {
        gameObject.tag = "LootBox";
        GenerateCells();
    }
    
    public void Init(InventoryHandler handler)
    {
        handler.LootBoxSlotsContainer.Init(this, Cells);
    }

    private void GenerateCells()
    {
        foreach (var item in _set.Items)
            Cells.Add(new InventoryCell(item.Item, Random.Range(item.MinimalCount, item.MaximalCount)));
    }

    public void Open(InventoryHandler handler)
    {
        handler.OpenLootBoxPanel();
        Init(handler);
    }

    private void CheckCells()
    {
        if(!_demolishing) return;
        if(Cells.Count == 0) Destroy(gameObject);
    }
    
    public void RemoveCell(Item item, int count)
    {
        foreach (var cell in Cells)
        {
            if(cell.Item == null || item == null) return;
            if (cell.Item.Id == item.Id && cell.Count == count)
            {
                Cells.Remove(cell);
                CheckCells();
                return;
            }
        }
    }
}