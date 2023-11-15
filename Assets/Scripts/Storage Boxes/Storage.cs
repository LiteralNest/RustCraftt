using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Storage : NetworkBehaviour
{
    private NetworkList<Vector3> _itemsNetData;

    [FormerlySerializedAs("_cells")] [SerializeField]
    public List<InventoryCell> Cells;

    public SlotsDisplayer SlotsDisplayer { get; set; }

    [Header("Test")] [SerializeField] private InventoryCell _testAddingCell;

    private void Awake()
        => _itemsNetData = new NetworkList<Vector3>();

    private void Start()
    {
        gameObject.tag = "LootBox";
    }

    #region virtual

    public virtual void InitBox()
        => SaveNetData();

    public virtual void CheckCells()
    {
    }

    public virtual void Open(InventoryHandler handler)
    {
        Appear();
        SlotsDisplayer.AssignStorage(this);
        SlotsDisplayer.InitCells();
        SlotsDisplayer.DisplayCells();
    }

    protected virtual void Appear()
        => ActiveInvetoriesHandler.singleton.AddActiveInventory(this);

    #endregion
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
            InitBox();
        else
            ConvertWebData();
        // _itemsNetData.OnListChanged += ConvertWebData;
        base.OnNetworkSpawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetItemServerRpc(int id)
    {
        if (IsServer)
            _itemsNetData[id] = new Vector3Int(-1, 0, -1);
        ConvertWebData();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetItemServerRpc(int cellId, int itemId, int count, int hp = -1, bool shouldSaveNetData = true, bool shouldDisplay = true)
    {
        if (IsServer)
            _itemsNetData[cellId] = new Vector3Int(itemId, count, hp);
        if(shouldSaveNetData)
            ConvertWebData(shouldDisplay);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddItemCountServerRpc(int cellId, int itemId, int count)
    {
        if (IsServer)
        {
            _itemsNetData[cellId] = new Vector3Int(itemId, (int)_itemsNetData[cellId].y);
            _itemsNetData[cellId] += new Vector3Int(0, count);
        }

        ConvertWebData();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveItemCountServerRpc(int id, int count)
    {
        if (IsServer)
        {
            _itemsNetData[id] -= new Vector3Int(0, count, -1);
            if (_itemsNetData[id].y <= 0)
                _itemsNetData[id] = new Vector3Int(-1, 0, -1);
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

    private void ConvertWebData(bool shouldDisplayeCells = true)
    {
        Cells = CustomDataSerializer.GetConvertedCellsList(_itemsNetData);
        CheckCells();
        if (SlotsDisplayer == null) return;
        if(shouldDisplayeCells)
            SlotsDisplayer.DisplayCells();
    }

    public virtual bool CanAddItem(Item item)
        => true;

    protected void RemoveItem(Item item, int count)
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            if (Cells[i].Item == item)
                RemoveItemCountServerRpc(i, count);
        }
    }

    [ContextMenu("DebugNetCells")]
    private void DebugNetCells()
    {
        foreach (var cell in _itemsNetData)
            Debug.Log("Id: " + cell.x + " Count: " + cell.y + " HP: " + cell.z);
    }

    [ContextMenu("Test")]
    private void AddTestCell()
        => AddItemToDesiredSlotServerRpc(_testAddingCell.Item.Id, _testAddingCell.Count);
}