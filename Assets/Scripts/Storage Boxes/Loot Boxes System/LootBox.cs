using System.Collections.Generic;
using Storage_Boxes;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LootBox : Storage
{
    [SerializeField] private List<LootBoxGeneratingSet> _setsPool = new List<LootBoxGeneratingSet>();

    public override void InitBox()
        => GenerateCells();

    public override void Open(InventoryHandler handler)
    { 
        handler.InventoryPanelsDisplayer.OpenLootBoxPanel();
        SlotsDisplayer = handler.LootBoxSlotsDisplayer;
        base.Open(handler);
    }
    
    public override void CheckCells()
    {
        CheckCellsServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void CheckCellsServerRpc()
    {
        if(!IsServer) return;
        foreach (var cell in Cells)
            if (cell.Item != null)
                return;
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
    
    private LootBoxGeneratingSet GetRandomSet()
        => _setsPool[Random.Range(0, _setsPool.Count)];
    
    private void GenerateCells()
    {
        var set = GetRandomSet();
        for (int i = 0; i < set.Items.Count; i++)
        {
            Cells[i].Item = set.Items[i].Item;
            Cells[i].Count = Random.Range(set.Items[i].MinimalCount, set.Items[i].MaximalCount + 1);
        }
        SaveNetData();
    }
}