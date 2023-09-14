using System.Collections.Generic;

public class InventorySlotsContainer : SlotsContainer
{
    public static InventorySlotsContainer singleton { get; set; }

    private void Awake()
        => singleton = this;

    private void Start()
        => TryLoadInventory();
    
    private void ConvertWebDataToList(List<SendingDataField> data)
    {
        ResetCells();
        int i = 0;
        foreach (var cell in data)
        {
            Cells[i] = new InventoryCell(ItemsContainer.singleton.GetItemById(cell.ItemId), cell.Count);
            i++;
        }
        _cellsDisplayer.DisplayCells();
    }
    
    private async void TryLoadInventory()
    {
        List<SendingDataField> data = await FirebaseInventoryDataSender.singleton.TryLoadData();
        ConvertWebDataToList(data);
    }
    
    public override void AddItemToDesiredSlot(Item item, int count)
    {
        GlobalEventsContainer.InventoryItemAdded?.Invoke(new InventoryCell(item, count));
        base.AddItemToDesiredSlot(item, count);
        GlobalEventsContainer.InventoryDataShouldBeSaved?.Invoke(Cells);
        GlobalEventsContainer.InventoryDataChanged?.Invoke();
    }

    public override void DeleteSlot(Item item, int count)
    {
        base.DeleteSlot(item, count);
        GlobalEventsContainer.InventoryDataShouldBeSaved?.Invoke(Cells);
        GlobalEventsContainer.InventoryDataChanged?.Invoke();
    }

    public override void RemoveItemCountAt(Item item, int count, int index)
    {
        base.RemoveItemCountAt(item, count, index);
        GlobalEventsContainer.InventoryItemRemoved?.Invoke(new InventoryCell(item, count));
    }
}