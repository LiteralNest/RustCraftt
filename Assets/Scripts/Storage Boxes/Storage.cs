using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Storage : NetworkBehaviour
{
    [field:SerializeField] private NetworkList<Vector2> _itemsNetData;

    [FormerlySerializedAs("_cells")] [SerializeField]
    public List<InventoryCell> Cells;

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

    private void Awake()
        => _itemsNetData = new NetworkList<Vector2>();
    
    private void Start()
        => gameObject.tag = "LootBox";

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


    protected void SaveNetData()
        => CustomDataSerializer.SetConvertedItemsList(Cells, _itemsNetData);

    public virtual void ConvertWebData()
    {
        Cells = CustomDataSerializer.GetConvertedCellsList(_itemsNetData);
        CheckCells();
    }

    public virtual void ConvertWebData(NetworkListEvent<Vector2> changeEvent)
    {
        if (!IsServer)
            ConvertWebData();
    }

    [ContextMenu("DebugNetCells")]
    private void DebugNetCells()
    {
        foreach (var cell in _itemsNetData)
            Debug.Log("Id: " + cell.x + " Count: " + cell.y);
    }
}