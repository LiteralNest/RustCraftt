using System.Collections.Generic;
using UnityEngine;
using Storage_System;
using Unity.Netcode;

namespace Inventory_System
{
    public static class InventoryHelper
    {
        private static CustomSendingInventoryDataCell[] GetNewGeneratedArray(
            CustomSendingInventoryDataCell[] inputArray)
        {
            var res = new CustomSendingInventoryDataCell[inputArray.Length];
            for (int i = 0; i < inputArray.Length; i++)
                res[i] = new CustomSendingInventoryDataCell(inputArray[i].Id, inputArray[i].Count, inputArray[i].Hp);
            return res;
        }

        public static void ResetCell(int cellId, NetworkVariable<CustomSendingInventoryData> data)
        {
            var cells = data.Value.Cells;
            cells[cellId] = new CustomSendingInventoryDataCell(-1, 0, -1);
            data.Value = new CustomSendingInventoryData(cells);
        }

        private static void AddCountToCell(int cellId, int itemId, int count,
            NetworkVariable<CustomSendingInventoryData> data)
        {
            var cells = GetNewGeneratedArray(data.Value.Cells);
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
            var cells = GetNewGeneratedArray(data.Value.Cells);
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
            var cells = GetNewGeneratedArray(data.Value.Cells);
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

            data.Value = new CustomSendingInventoryData(cachedData.Value.Cells);
        }

        private static int GetFreeCellId(NetworkVariable<CustomSendingInventoryData> data, Vector2Int range = default)
        {
            if(range == default) range = new Vector2Int(0, data.Value.Cells.Length);
            for (int i = range.x; i < range.y; i++)
            {
                if (data.Value.Cells[i].Id == -1)
                    return i;
            }

            return -1;
        }

        public static int GetDesiredCellId(int itemId, int count, NetworkVariable<CustomSendingInventoryData> data,
            Vector2Int range = default)
        {
            if (range == default) range = new Vector2Int(0, data.Value.Cells.Length);
            for (int i = range.x; i < range.y; i++)
            {
                if (data.Value.Cells[i].Id != itemId) continue;
                if (data.Value.Cells[i].Count >= count)
                    return i;
            }

            return GetFreeCellId(data);
        }

        public static void AddItemToDesiredSlot(int itemId, int count, NetworkVariable<CustomSendingInventoryData> data,
            Vector2Int range)
        {
            var cachedCount = count;
            if (range == Vector2Int.one) range.y = data.Value.Cells.Length;
            var cells = data.Value.Cells;
            for (int i = range.x; i < range.y; i++)
            {
                if (cells[i].Id == -1) continue;
                var item = ItemFinder.singleton.GetItemById(cells[i].Id);
                if (cells[i].Id != itemId || cells[i].Count >= item.StackCount) continue;
                int sum = cells[i].Count + cachedCount;
                if (sum > item.StackCount)
                {
                    var addingCount = item.StackCount - cells[i].Count;
                    AddCountToCell(i, itemId, addingCount, data);
                    cachedCount -= addingCount;
                }
                else if (sum < item.StackCount)
                {
                    AddCountToCell(i, itemId, cachedCount, data);
                    cachedCount = 0;
                }

                if (cachedCount <= 0) break;
            }

            while (cachedCount > 0)
            {
                var cellId = GetFreeCellId(data, range);
                if (cellId == -1) break;
                var item = ItemFinder.singleton.GetItemById(itemId);
                if (item.StackCount > cachedCount)
                {
                    SetItem(cellId, new CustomSendingInventoryDataCell(itemId, count, 100), data);
                    cachedCount = 0;
                }
                else
                {
                    SetItem(cellId, new CustomSendingInventoryDataCell(itemId, item.StackCount, 100), data);
                    cachedCount -= item.StackCount;
                }
            }
        }

        public static bool EnoughMaterials(List<InventoryCell> inputSlots,
            NetworkVariable<CustomSendingInventoryData> data)
        {
            NetworkVariable<CustomSendingInventoryData> cachedData =
                new NetworkVariable<CustomSendingInventoryData>();
            cachedData.Value = new CustomSendingInventoryData(GetNewGeneratedArray(data.Value.Cells));
            for (int i = 0; i < data.Value.Cells.Length; i++)
                cachedData.Value.Cells[i] =
                    new CustomSendingInventoryDataCell(data.Value.Cells[i].Id, data.Value.Cells[i].Count, -1);
            var cells = cachedData.Value.Cells;

            for (int i = 0; i < inputSlots.Count; i++)
            {
                var neededSlot = inputSlots[i];
                if (neededSlot.Item == null) continue;
                for (int j = 0; j < cells.Length; j++)
                {
                    var inventorySlot = cells[j];
                    if (inventorySlot.Id == -1) continue;
                    if (inventorySlot.Id == neededSlot.Item.Id && inventorySlot.Count >= neededSlot.Count)
                    {
                        MinusCellCount(j, neededSlot.Count, cachedData);
                        inputSlots.RemoveAt(i);
                        i--;
                        if (inputSlots.Count == 0) return true;
                        break;
                    }
                }
            }

            if (inputSlots.Count == 0) return true;
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
}