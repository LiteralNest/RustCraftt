using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LootBox : Storage
{
    [SerializeField] private LootBoxGeneratingSet _set;

    public override void InitBox()
        => InitLootBox();

    public override void Open(InventoryHandler handler)
        => OpenBox(handler);
    
    public override void CheckCells()
    {
        foreach (var cell in _cells)
            if (cell.Item != null)
                return;
        Destroy(gameObject);
    }
    
    private async void InitLootBox()
    {
        BoxId.Value = await WebServerDataHandler.singleton.RegistrateNewLootBox();
        GenerateCells();
    }

    private async Task LoadCells()
    {
        var cells = await WebServerDataHandler.singleton.LoadLootBoxData(BoxId.Value);
        AssignCells(cells);
    }
    
    private void GenerateCells()
    {
        for (int i = 0; i < _set.Items.Count; i++)
        {
            _cells[i].Item = _set.Items[i].Item;
            _cells[i].Count = Random.Range(_set.Items[i].MinimalCount, _set.Items[i].MaximalCount);
        }
        GlobalEventsContainer.LootBoxDataShouldBeSaved?.Invoke(_cells, BoxId.Value);
    }

    private async void OpenBox(InventoryHandler handler)
    {
        await LoadCells();
        handler.LootboxSlotsContainer.InitCells(_cells, this);
        handler.OpenLootBoxPanel();
    }
}