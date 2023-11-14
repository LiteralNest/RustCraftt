using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Storage : NetworkBehaviour
{
    private NetworkList<Vector2> _itemsNetData;

    [FormerlySerializedAs("_cells")] [SerializeField]
    public List<InventoryCell> Cells;

    public SlotsDisplayer SlotsDisplayer { get; set; }

    [Header("Test")] [SerializeField] private InventoryCell _testAddingCell;

    #region abstract

    public virtual void InitBox()
    {
        SaveNetData();
    }

    public virtual void CheckCells()
    {
    }

    public abstract void Open(InventoryHandler handler);

    #endregion

    #region virtual

    protected virtual void Appear()
        => ActiveInvetoriesHandler.singleton.AddActiveInventory(this);

    #endregion

    private void Awake()
        => _itemsNetData = new NetworkList<Vector2>();

    private void Start()
    {
        Appear();
        gameObject.tag = "LootBox";
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            InitBox();
        else
            ConvertWebData();
        _itemsNetData.OnListChanged += ConvertWebData;
        base.OnNetworkSpawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetItemServerRpc(int id)
    {
        if (IsServer)
            _itemsNetData[id] = new Vector2Int(-1, 0);
        ConvertWebData();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetItemServerRpc(int cellId, int itemId, int count)
    {
        if (IsServer)
            _itemsNetData[cellId] = new Vector2Int(itemId, count);
        ConvertWebData();
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SetItemToCellServerRpc(int cellId, int itemId, int count)
    {
        if (IsServer)
        {
            _itemsNetData[cellId] = new Vector2Int(itemId,  (int)_itemsNetData[cellId].y);
            _itemsNetData[cellId] += new Vector2Int(0, count);
        }
        ConvertWebData();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveItemCountServerRpc(int id, int count)
    {
        if (IsServer)
        {
            _itemsNetData[id] -= new Vector2Int(0, count);
            if (_itemsNetData[id].y <= 0)
                _itemsNetData[id] = new Vector2Int(-1, 0);
        }

        ConvertWebData();
    }

    public void RemoveItems(List<InventoryCell> cells)
    {
        foreach (var cell in cells)
            RemoveItemCountServerRpc(cell.Item.Id, cell.Count);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddItemToDesiredSlotServerRpc(int itemId, int count)
    {
        if (IsServer)
        {
            InventoryHelper.AddItemToDesiredSlot(itemId, count, Cells);
            SaveNetData();
        }
        ConvertWebData();
    }

    protected void SaveNetData()
        => CustomDataSerializer.SetConvertedItemsList(Cells, _itemsNetData);

    public bool EnoughMaterials(List<InventoryCell> inputCells)
        => InventoryHelper.EnoughMaterials(inputCells, Cells);

    public int GetItemCount(int id)
        => InventoryHelper.GetItemCount(id, Cells);

    private void ConvertWebData()
    {
        Cells = CustomDataSerializer.GetConvertedCellsList(_itemsNetData);
        CheckCells();
        SlotsDisplayer.DisplayCells();
    }

    private void ConvertWebData(NetworkListEvent<Vector2> changeEvent)
    {
        ConvertWebData();
    }

    public virtual bool CanAddItem(Item item)
        => true;

    [ContextMenu("DebugNetCells")]
    private void DebugNetCells()
    {
        foreach (var cell in _itemsNetData)
            Debug.Log("Id: " + cell.x + " Count: " + cell.y);
    }

    [ContextMenu("Test")]
    private void AddTestCell()
        => AddItemToDesiredSlotServerRpc(_testAddingCell.Item.Id, _testAddingCell.Count);
}