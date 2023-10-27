using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LootBox : NetworkBehaviour
{
    [field: SerializeField]
    public NetworkVariable<int> LootBoxId { get; private set; } = new(-1,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [SerializeField] private LootBoxGeneratingSet _set;
    [SerializeField] private List<InventoryCell> _cells;

    private void Start()
    {
        gameObject.tag = "LootBox";
        LoadCells();
    }

    public override void OnNetworkSpawn()
    {
#if UNITY_SERVER
        InitLootBox();
#endif
        
        base.OnNetworkSpawn();
    }

    private async void InitLootBox()
    {
        LootBoxId.Value = await WebServerDataHandler.singleton.RegistrateNewLootBox();
        GenerateCells();
    }

    private void AssignCells(List<InventorySendingDataField> dataCells)
    {
        for (int i = 0; i < dataCells.Count; i++)
        {
            _cells[i].Item = ItemsContainer.singleton.GetItemById(dataCells[i].ItemId);
            _cells[i].Count = dataCells[i].Count;
        }
    }
    
    private async void LoadCells()
    {
        var cells = await WebServerDataHandler.singleton.LoadLootBoxData(LootBoxId.Value);
        AssignCells(cells);
    }
    
    private void GenerateCells()
    {
        foreach (var item in _set.Items)
            _cells.Add(new InventoryCell(item.Item, Random.Range(item.MinimalCount, item.MaximalCount)));
        GlobalEventsContainer.LootBoxDataShouldBeSaved?.Invoke(_cells, LootBoxId.Value);
    }

    public void Open(InventoryHandler handler)
    {
        handler.OpenLootBoxPanel();
        handler.LootBoxSlotsContainer.InitCells(_cells);
    }

    private void CheckCells()
    {
        foreach (var cell in _cells)
            if (cell.Item != null)
                return;
        Destroy(gameObject);
    }

    public void RemoveCell(Item item, int count)
    {
        CheckCells();
        // foreach (var cell in Cells)
        // {
        //     if(cell.Item == null || item == null) return;
        //     if (cell.Item.Id == item.Id && cell.Count == count)
        //     {
        //         Cells.Remove(cell);
        //         CheckCells();
        //         return;
        //     }
        // }
    }
}