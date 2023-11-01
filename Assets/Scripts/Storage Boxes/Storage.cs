using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Storage : NetworkBehaviour
{
    [field: SerializeField]
    public NetworkVariable<int> BoxId { get; private set; } = new(-1,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    [SerializeField] protected List<InventoryCell> _cells;

    #region abstract
    
    public abstract void InitBox();
    
    public abstract void CheckCells();

    public abstract void Open(InventoryHandler handler);

    #endregion
    
    private void Start()
    {
        gameObject.tag = "LootBox";
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
            InitBox();
        base.OnNetworkSpawn();
    }
    
    protected void AssignCells(List<InventorySendingDataField> dataCells)
    {
        int i = 0;
        for (i = 0; i < dataCells.Count; i++)
        {
            _cells[i].Item = ItemsContainer.singleton.GetItemById(dataCells[i].ItemId);
            _cells[i].Count = dataCells[i].Count;
        }

        for (int j = i; j < _cells.Count; j++)
        {
            _cells[i].Item = null;
            _cells[i].Count = 0;
        }
    }

    public void AssignCellsAndSendData(List<InventoryCell> inputCells)
    {
        _cells = new List<InventoryCell>(inputCells);
        WebServerDataHandler.singleton.SaveLootBoxData(_cells, BoxId.Value);
        CheckCells();
    }
}
