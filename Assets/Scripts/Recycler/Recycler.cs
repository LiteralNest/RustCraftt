using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Recycler : MonoBehaviour
{
    [Header("Main Params")] [SerializeField]
    private float _recyclingTime = 1;
    
    [Header("Cells")]
    [SerializeField] private List<InventoryCell> _cells = new List<InventoryCell>();
    [SerializeField] private List<RecyclingItem> _avaliableItems = new List<RecyclingItem>();
    
    private RecyclerSlotsContainer _recyclerSlotsContainer;
    private bool _turned;
    private bool _recycling;

    private void Start()
        => gameObject.tag = "Recycler";

    private void Update()
    {
        if(!_turned) return;
        TryRecycle();
    }
    
    public void Open(InventoryHandler handler)
    {
        handler.RecyclerSlotsContainer.InitCells(_cells);
        _recyclerSlotsContainer = handler.RecyclerSlotsContainer;
        handler.OpenRecyclerPanel();
        _recyclerSlotsContainer.Init(this);
    }

    private RecyclingItem GetRecyclingItemById(int id)
    {
        foreach (var item in _avaliableItems)
        {
            if (item.Id == id)
                return item;
        }
        return null;
    }
    
    private void ResetCell(InventoryCell cell)
    {
        cell.Item = null;
        cell.Count = 0;
        _recyclerSlotsContainer.InitCells(_cells);
    }
    
    private void RemoveItem(Item item, int count)
    {
        foreach (var cell in _cells)
        {
            if (cell.Item == item)
            {
                cell.Count -= count;
                if(cell.Count > 0) return;
                ResetCell(cell);
            }
        }
    }
    
    private async void RecycleItem(RecyclingItem item)
    {
        await Task.Delay((int)(_recyclingTime * 1000));
        if(!_recycling) return;
        List<InventoryCell> recyclingCells = new List<InventoryCell>();
        recyclingCells = _cells.GetRange(5, 5);
        foreach (var cell in item.Cells)
        {
            var rand = Random.Range(cell.ItemsRange.x, cell.ItemsRange.y);
            var desiredCell = InventoryHelper.GetDesiredCell(cell.ResultItem, rand, recyclingCells);
            if (desiredCell == null)
            {
                _recycling = false;
                return;
            }
            desiredCell.Item = cell.ResultItem;
            desiredCell.Count += rand;
        }
        RemoveItem(item, 1);
        _recyclerSlotsContainer.InitCells(_cells);
        _recycling = false;
    }

    private void TryRecycle()
    {
        if(_recycling) return;
        var cells = _cells.GetRange(0, 5);
        foreach (var cell in cells)
        {
            if (!cell.Item || !GetRecyclingItemById(cell.Item.Id)) continue;
            RecycleItem(GetRecyclingItemById(cell.Item.Id));
            _recycling = true;
        }
    }

    public void SetTurned(bool value)
    {
        _turned = value;
        if (!value)
            _recycling = value;
    }
}