using System.Collections.Generic;
using System.Threading.Tasks;
using Storage_Boxes;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Recycler : Storage
{
    public NetworkVariable<bool> Turned = new NetworkVariable<bool>(false);

    [Header("Main Params")] [SerializeField]
    private float _recyclingTime = 1;
    
    [Header("Cells")]
    [SerializeField] private List<RecyclingItem> _avaliableItems = new List<RecyclingItem>();

    [Header("Audio")]
    [SerializeField] private AudioSource _source;
    
    private bool _recycling;

    private void Start()
        => gameObject.tag = "Recycler";

    private void Update()
    {
        if(!Turned.Value) return;
        TryRecycle();
    }

    public override void Open(InventoryHandler handler)
    {
        handler.InventoryPanelsDisplayer.OpenRecyclerPanel();
        SlotsDisplayer = handler.RecyclerSlotsDisplayer;
        base.Open(handler);
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
    
    private void RemoveItem(Item item, int count)
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            if(Cells[i].Item == item)
                RemoveItemCountServerRpc(i, count);
        }
    }
    
    private async void RecycleItem(RecyclingItem item)
    {
        await Task.Delay((int)(_recyclingTime * 1000));
        if(!_recycling) return;
        List<InventoryCell> recyclingCells = new List<InventoryCell>();
        recyclingCells = Cells.GetRange(5, 5);
        foreach (var cell in item.Cells)
        {
            var rand = Random.Range(cell.ItemsRange.x, cell.ItemsRange.y);
            var desiredCell = 5 + InventoryHelper.GetDesiredCellId(cell.ResultItem.Id, recyclingCells);
            if (desiredCell == -1)
            {
                _recycling = false;
                return;
            }
            SetItemServerRpc(desiredCell, cell.ResultItem.Id, rand);
        }
        RemoveItem(item, 1);
        _recycling = false;
    }

    private void TryRecycle()
    {
        if(!IsServer) return;
        if(_recycling) return;
        var cells = Cells.GetRange(0, 5);
        foreach (var cell in cells)
        {
            if (!cell.Item || !GetRecyclingItemById(cell.Item.Id)) continue;
            RecycleItem(GetRecyclingItemById(cell.Item.Id));
            _recycling = true;
            return;
        }
        SetTurnedServerRpc(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetTurnedServerRpc(bool value)
    {
        if (IsServer)
        {
            Turned.Value = value;
        }
        _source.Play();
        if (!value)
        {
            _source.Stop();
            _recycling = value;
        }
    }
}