using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LootBox : Storage
{
    [SerializeField] private LootBoxGeneratingSet _set;

    public override void InitBox()
        => GenerateCells();

    public override void Open(InventoryHandler handler)
        => OpenBox(handler);
    
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
    
    private void GenerateCells()
    {
        for (int i = 0; i < _set.Items.Count; i++)
        {
            Cells[i].Item = _set.Items[i].Item;
            Cells[i].Count = Random.Range(_set.Items[i].MinimalCount, _set.Items[i].MaximalCount);
        }
        SaveNetData();
    }

    private void OpenBox(InventoryHandler handler)
    {
        handler.LootboxSlotsContainer.InitCells(Cells, this);
        handler.OpenLootBoxPanel();
    }
}