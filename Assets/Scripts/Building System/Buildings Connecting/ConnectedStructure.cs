using System.Collections.Generic;
using Building_System.Blocks;
using Inventory_System;
using UnityEngine;
using System.Collections;
using Storage_System;

namespace Building_System.Buildings_Connecting
{
    public class ConnectedStructure : MonoBehaviour
    {
        [field: SerializeField] public List<BuildingBlock> Blocks { get; private set; } = new List<BuildingBlock>();

        [field: SerializeField]
        public List<ToolClipboard> TargetClipBoards { get; private set; } = new List<ToolClipboard>();

        [SerializeField] private float _decayingIterationTime = 1f;

        private void Start()
        {
            StartCoroutine(DecayRoutine());
        }

        private void GetBlock(List<BuildingBlock> _blocks)
        {
            foreach (var block in _blocks)
            {
                block.transform.SetParent(transform);
                block.BuildingConnector.SetNewStructure(this);
            }
        }

        public void MigrateBlocks(ConnectedStructure structure)
        {
            if (Blocks.Count != 0)
            {
                structure.GetBlock(Blocks);
                Blocks.Clear();
            }

            Destroy(gameObject);
        }

        #region Counting Remaining Time

        private List<InventoryCell> GetConvertedItemsList(List<CustomSendingInventoryDataCell> data)
        {
            var res = new List<InventoryCell>();
            foreach (var cell in data)
            {
                if(cell.Id == -1) continue;
                res.Add(new InventoryCell(ItemFinder.singleton.GetItemById(cell.Id), cell.Count));
            }
                
            return res;
        }
        
        private bool TryMinusItemsPreSecond(List<InventoryCell> delCells,
            List<InventoryCell> data)
        {
            var deletingCells = new List<InventoryCell>(delCells);
            for (int i = 0; i < deletingCells.Count; i++)
            {
                for (int j = 0; j < data.Count; j++)
                {
                    if (deletingCells[i].Item == null || data[j].Item == null) continue;
                    if (deletingCells[i].Item.Id == data[j].Item.Id)
                    {
                        if (deletingCells.Count <= data[j].Count)
                        {
                            if (data[j].Count <= 0)
                            {
                                data.RemoveAt(j);
                                j--;
                            }
                            else
                                data[j].Count -= deletingCells.Count;
                            deletingCells.RemoveAt(i);
                            i--;
                            if (deletingCells.Count == 0) return true;
                            continue;
                        }
                        deletingCells[i].Count -= data[j].Count;
                        if (deletingCells.Count == 0) return true;
                    }
                }
                if (deletingCells.Count == 0) return true;
            }

            return false;
        }

        private List<InventoryCell> GetRemovingCells()
        {
            List<InventoryCell> res = new List<InventoryCell>();
            foreach (var block in Blocks)
            {
                foreach (var cell in block.CurrentBlock.CellsForRemovingPerTime)
                    res.Add(cell);
            }
            return res; 
        }

        private List<InventoryCell> GetClipBoardCells()
        {
            List<CustomSendingInventoryDataCell> cells = new List<CustomSendingInventoryDataCell>();
            foreach (var clipboard in TargetClipBoards)
                cells.AddRange(clipboard.ItemsNetData.Value.Cells);
            return GetConvertedItemsList(cells);
        }
        
        public int GetAvaliableMinutes()
        {
            var res = 0;
            List<InventoryCell> deletingCells = GetRemovingCells();
            var list = GetClipBoardCells();
            while (TryMinusItemsPreSecond(deletingCells, list))
                res++;
            return res;
        }

        private void AddSlotToInventory(InventoryCell addingCell, List<InventoryCell> data)
        {
            foreach (var slot in data)
            {
                if (slot.Item.Id == addingCell.Item.Id)
                {
                    slot.Count += addingCell.Count;
                    return;
                }
            }
            data.Add(addingCell);
        }
        
        private List<InventoryCell> GetStackedList(List<InventoryCell> inputList)
        {
            var res = new List<InventoryCell>();
            if (inputList.Count == 0) return res;
            res.Add(inputList[0]);
            inputList.RemoveAt(0);
            foreach (var cell in inputList)
                AddSlotToInventory(cell, res);
            return res;
        }
        
        public List<InventoryCell> GetNeededResourcesForDay()
        {
            var res = new List<InventoryCell>();
            var inputList = GetStackedList(GetRemovingCells());
            foreach (var cell in inputList)
                res.Add(new InventoryCell(cell.Item, cell.Count * 24));
            return res;
        }
        
        #endregion


        private bool ThereIsEnoughMaterials(List<InventoryCell> comparingCells)
        {
            if (TargetClipBoards.Count == 0) return false;
            if (InventoryHelper.EnoughMaterials(comparingCells, TargetClipBoards[0].ItemsNetData))
            {
                foreach (var cell in comparingCells)
                    InventoryHelper.RemoveItemCount(cell.Item.Id, cell.Count, TargetClipBoards[0].ItemsNetData);
                return true;
            }

            return false;
        }

        private void Decay()
        {
            foreach (var block in Blocks)
            {
                if (ThereIsEnoughMaterials(block.CurrentBlock.CellsForRemovingPerTime))
                    block.RestoreHealth(block.StartHp / 10);
                block.GetDamage(block.StartHp / 10);
            }
        }

        private IEnumerator DecayRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_decayingIterationTime);
                Decay();
            }
        }
    }
}