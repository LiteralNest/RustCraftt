using System.Collections.Generic;

[System.Serializable]
public struct SendingDataField
{
    public int Count;
    public int ItemId;
}

[System.Serializable]
public struct SendingData
{
    public List<SendingDataField> Cells;
}

public static class WebDataConverter
{
    public static SendingData GetConvertedSendingData(List<InventoryCell> inputCells)
    {
        SendingData data = new SendingData();
        List<SendingDataField> cells = new List<SendingDataField>();
        foreach (var cell in inputCells)
        {
            if (cell.Item == null) continue;
            cells.Add(new SendingDataField { Count = cell.Count, ItemId = cell.Item.Id });
        }

        data.Cells = cells;
        return data;
    }
}
