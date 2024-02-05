using System.Collections.Generic;
using Inventory_System;
using UnityEngine;
using System.Collections;
using Building_System.Building.Blocks;
using Tool_Clipboard;
using Unity.Netcode;

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
            if(!NetworkManager.Singleton.IsServer) return;
            StartCoroutine(DecayRoutine());
        }

        public void RemoveBlock(BuildingBlock block)
        {
            Blocks.Remove(block);
            if (Blocks.Count <= 0)
                Destroy(gameObject);
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
                block.Decay(block.StartHp / 10);
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