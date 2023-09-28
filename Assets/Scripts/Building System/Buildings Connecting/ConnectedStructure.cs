using System.Collections.Generic;
using UnityEngine;

public class ConnectedStructure : MonoBehaviour
{
    [field: SerializeField] public List<BuildingBlock> Blocks { get; private set; } = new List<BuildingBlock>();

    [field: SerializeField]
    public List<ToolClipboard> TargetClipBoards { get; private set; } = new List<ToolClipboard>();

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
}