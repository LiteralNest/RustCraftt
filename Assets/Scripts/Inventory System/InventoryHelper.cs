using System.Collections.Generic;
using Storage_System;
using Unity.Netcode;

public static class InventoryHelper
{
    public static void ResetCell(int cellId, NetworkVariable<CustomSendingInventoryData> data)
    {
        var cells = data.Value.Cells;
        cells[cellId] = new CustomSendingInventoryDataCell(-1, 0, -1);
        data.Value = new CustomSendingInventoryData(cells);
    }

    public static void AddCountToCell(int cellId, int itemId, int count,
        NetworkVariable<CustomSendingInventoryData> data)
    {
        var cells = data.Value.Cells;
        cells[cellId].Id = itemId;
        cells[cellId].Count += count;
        data.Value = new CustomSendingInventoryData(cells);
    }

    public static void ResetItems(NetworkVariable<CustomSendingInventoryData> data)
    {
        NetworkVariable<CustomSendingInventoryData> cells = data;
        for (int i = 0; i < cells.Value.Cells.Length; i++)
            ResetCell(i, cells);
        data.Value = cells.Value;
    }

    public static void SetItem(int cellId, CustomSendingInventoryDataCell dataCell,
        NetworkVariable<CustomSendingInventoryData> data)
    {
        var cells = data.Value.Cells;
        cells[cellId] = dataCell;
        data.Value = new CustomSendingInventoryData(cells);
    }

    public static void AddCell(CustomSendingInventoryDataCell dataCell,
        NetworkVariable<CustomSendingInventoryData> data)
    {
        var cells = data.Value.Cells;
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].Id == -1)
                cells[i] = dataCell;
        }

        data.Value = new CustomSendingInventoryData(cells);
    }

    public static void MinusCellCount(int cellId, int count,
        NetworkVariable<CustomSendingInventoryData> data)
    {
        var cells = data.Value.Cells;
        cells[cellId].Count -= count;
        if (cells[cellId].Count <= 0)
            cells[cellId] = new CustomSendingInventoryDataCell(-1, 0, -1);
        data.Value = new CustomSendingInventoryData(cells);
    }


    public static void RemoveItemCount(int itemId, int count, NetworkVariable<CustomSendingInventoryData> data)
    {
        NetworkVariable<CustomSendingInventoryData> cachedData =
            new NetworkVariable<CustomSendingInventoryData>(data.Value);
        var cells = data.Value.Cells;
        int cachedCount = count;
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].Id == itemId)
            {
                cachedCount -= cells[i].Count;
                MinusCellCount(i, cells[i].Count, cachedData);
                if (cachedCount <= 0) break;
            }
        }

        data.Value = cachedData.Value;
    }

    private static int GetFreeCellId(NetworkVariable<CustomSendingInventoryData> data)
    {
        for (int i = 0; i < data.Value.Cells.Length; i++)
        {
            if (data.Value.Cells[i].Id == -1)
                return i;
        }

        return -1;
    }

    public static int GetDesiredCellId(int itemId, int count, NetworkVariable<CustomSendingInventoryData> data)
    {
        for (int i = 0; i < data.Value.Cells.Length; i++)
        {
            if (data.Value.Cells[i].Id != itemId) continue;
            if (data.Value.Cells[i].Count >= count)
                return i;
        }

        return GetFreeCellId(data);
    }

    public static void AddItemToDesiredSlot(int itemId, int count, NetworkVariable<CustomSendingInventoryData> data)
    {
        var cellId = GetDesiredCellId(itemId, count, data);
        AddCountToCell(cellId, itemId, count, data);
    }

    public static bool EnoughMaterials(List<InventoryCell> inputSlots, NetworkVariable<CustomSendingInventoryData> data)
    {
        List<CustomSendingInventoryDataCell> dataCells = new List<CustomSendingInventoryDataCell>();
        foreach (var cell in inputSlots)
        {
            dataCells.Add(new CustomSendingInventoryDataCell(cell.Item.Id, cell.Count, cell.Hp));
        }

        var cells = data.Value.Cells;
        for (int i = 0; i < dataCells.Count; i++)
        {
            var slot = dataCells[i];
            if (slot.Id == -1) continue;
            for (int j = 0; j < cells.Length; j++)
            {
                var cell = cells[j];
                if (cell.Id == -1) continue;
                if (cell.Id == slot.Id && cell.Count >= slot.Count)
                {
                    MinusCellCount(j, slot.Count, data);
                    dataCells.Remove(slot);
                    i--;
                    if (dataCells.Count == 0) return true;
                }
            }
        }

        if (dataCells.Count == 0) return true;
        return false;
    }

    public static int GetItemCount(int itemId, NetworkVariable<CustomSendingInventoryData> data)
    {
        int res = 0;
        foreach (var cell in data.Value.Cells)
        {
            if (cell.Id == itemId)
                res += cell.Count;
        }

        return res;
    }
}