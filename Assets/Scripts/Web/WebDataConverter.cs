using System.Collections.Generic;

[System.Serializable]
public struct InventorySendingDataField
{
    public int Count;
    public int ItemId;
}

[System.Serializable]
public struct InventorySendingData
{
    public List<InventorySendingDataField> Cells;
}

public static class WebDataConverter
{
    public static InventorySendingData GetConvertedSendingData(List<InventoryCell> inputCells)
    {
        InventorySendingData data = new InventorySendingData();
        List<InventorySendingDataField> cells = new List<InventorySendingDataField>();
        foreach (var cell in inputCells)
        {
            if (cell.Item == null) continue;
            cells.Add(new InventorySendingDataField { Count = cell.Count, ItemId = cell.Item.Id });
        }

        data.Cells = cells;
        return data;
    }
}
