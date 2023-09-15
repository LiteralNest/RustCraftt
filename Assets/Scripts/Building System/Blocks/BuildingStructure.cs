using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BuildingStructure : MonoBehaviour
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public Vector3 StructureSize { get; private set; } = Vector3.one;
    [SerializeField] private List<BuildingBlock> _blocks = new List<BuildingBlock>();

    public List<InventoryCell> GetPlacingRemovingCells()
    {
        List<InventoryCell> res = new List<InventoryCell>();
        foreach (var block in _blocks)
            res.AddRange(block.NeededCellsForPlace);
        return res;
    }

    private void Start()
    {
        ConnectBlocks();
    }

    private List<ConnectedStructure> GetAddedStructures(List<ConnectedStructure> startedStructures,
        List<ConnectedStructure> addingStructures)
    {
        List<ConnectedStructure> res = startedStructures;
        foreach (var structure in addingStructures)
        {
            if (startedStructures.Contains(structure)) continue;
            res.Add(structure);
        }

        return res;
    }

    private async void ConnectBlocks()
    {
        await Task.Delay(100);
        ConnectedStructure currentStructure = null;
        List<ConnectedStructure> structures = new List<ConnectedStructure>();
        foreach (var block in _blocks)
        {
            structures = GetAddedStructures(structures, block.BuildingConnector.GetRelativeStructuresList());
        }

        int i = 0;

        if (structures.Count == 0)
            currentStructure = _blocks[0].BuildingConnector.GetInstantiatedStructure();
        else
        {
            currentStructure = structures[0];
            while (structures.Count > 1)
            {
                if (i == 0)
                {
                    i++;
                    continue;
                }
                structures[i].MigrateBlocks(currentStructure);
                structures.RemoveAt(i);
            }
            // foreach (var structure in structures)
            // {
            //     
            //     structure.MigrateBlocks(currentStructure);
            // }
        }

        foreach (var block in _blocks)
        {
            block.BuildingConnector.SetNewStructure(currentStructure);
        }

        Destroy(gameObject);
    }
}