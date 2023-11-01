using System.Collections.Generic;
using System.Threading.Tasks;
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
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
            InitLootBox();
        base.OnNetworkSpawn();
    }

    private async void InitLootBox()
    {
        LootBoxId.Value = await WebServerDataHandler.singleton.RegistrateNewLootBox();
        GenerateCells();
    }

    private void AssignCells(List<InventorySendingDataField> dataCells)
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
        WebServerDataHandler.singleton.SaveLootBoxData(_cells, LootBoxId.Value);
        CheckCells();
    }
    
    private async Task LoadCells()
    {
        var cells = await WebServerDataHandler.singleton.LoadLootBoxData(LootBoxId.Value);
        AssignCells(cells);
    }
    
    private void GenerateCells()
    {
        for (int i = 0; i < _set.Items.Count; i++)
        {
            _cells[i].Item = _set.Items[i].Item;
            _cells[i].Count = Random.Range(_set.Items[i].MinimalCount, _set.Items[i].MaximalCount);
        }
        GlobalEventsContainer.LootBoxDataShouldBeSaved?.Invoke(_cells, LootBoxId.Value);
    }

    public async void Open(InventoryHandler handler)
    {
        await LoadCells();
        handler.LootBoxSlotsContainer.InitCells(_cells, this);
        handler.OpenLootBoxPanel();
    }

    private void CheckCells()
    {
        foreach (var cell in _cells)
            if (cell.Item != null)
                return;
        Destroy(gameObject);
    }
}