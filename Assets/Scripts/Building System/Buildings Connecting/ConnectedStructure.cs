using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory_System;
using UnityEngine;

public class ConnectedStructure : MonoBehaviour
{
    [field: SerializeField] public List<BuildingBlock> Blocks { get; private set; } = new List<BuildingBlock>();

    [field: SerializeField]
    public List<ToolClipboard> TargetClipBoards { get; private set; } = new List<ToolClipboard>();

    [SerializeField] private float _decayingIterationTime = 1f;

    private void Start()
    {
        DecayAsync();
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

    public void AddClipBoard(ToolClipboard clipboard)
        => TargetClipBoards.Add(clipboard);

    private bool ThereIsEnoughMaterials(List<InventoryCell> comparingCells)
    {
        if (TargetClipBoards.Count == 0) return false;
        if (InventoryHelper.EnoughMaterials(comparingCells, TargetClipBoards[0].ItemsNetData))
        {
            foreach(var cell in comparingCells)
                InventoryHelper.RemoveItemCount(cell.Item.Id, cell.Count, TargetClipBoards[0].ItemsNetData);
            return true;
        }
        return false;
    }
    
    private void Decay()
    {
        foreach (var block in Blocks)
        {
            if(ThereIsEnoughMaterials(block.CurrentBlock.CellsForRemovingPerTime))
                block.RestoreHealth(block.StartHp / 10);
            block.GetDamage(block.StartHp / 10);
        }
    }

    private async void DecayAsync()
    {
        while (true)
        {
            await Task.Delay((int)(_decayingIterationTime * 1000));
            Decay();
        }
    }
}