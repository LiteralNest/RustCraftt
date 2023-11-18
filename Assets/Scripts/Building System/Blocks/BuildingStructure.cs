using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuildingStructure : MonoBehaviour
{
    [field: SerializeField] public NetworkObject NetworkObject { get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public Vector3 StructureSize { get; private set; } = Vector3.one;
    [SerializeField] private List<BuildingBlock> _blocks = new List<BuildingBlock>();

    public List<InventoryCell> GetPlacingRemovingCells()
    {
        List<InventoryCell> res = new List<InventoryCell>();
        foreach (var block in _blocks)
            res.AddRange(block.GetNeededCellsForPlace());
        return res;
    }
}