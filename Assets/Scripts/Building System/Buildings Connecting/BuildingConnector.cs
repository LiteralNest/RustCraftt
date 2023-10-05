using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BuildingConnector : MonoBehaviour
{
    [SerializeField] private ConnectedStructure _structurePrefab;
    [SerializeField] private BuildingBlock _buildingBlock;

    public ConnectedStructure CurrentStructure => _currentStructure;
    private ConnectedStructure _currentStructure;

    private List<BuildingConnector> _connectors = new List<BuildingConnector>();

    private void Start()
    {
        ConnectBlocks();
    }
    
    public ConnectedStructure GetInstantiatedStructure()
    {
        Vector3 position = transform.position;
        _currentStructure = Instantiate(_structurePrefab, position, Quaternion.identity);
        _currentStructure.GetComponent<NetworkObject>().Spawn();
        _buildingBlock.transform.GetComponent<NetworkObject>().TrySetParent(_currentStructure.transform);
        return _currentStructure;
    }
    
    public void SetNewStructure(ConnectedStructure structure)
    {
        structure.Blocks.Add(_buildingBlock);
        _currentStructure = structure;
        _buildingBlock.GetComponent<NetworkObject>().TrySetParent(_currentStructure.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ConnectingPoint") || !other.TryGetComponent(out BuildingConnector connector)) return;
        if (_connectors.Contains(connector)) return;
        _connectors.Add(connector);
    }
        
    public List<ConnectedStructure> GetRelativeStructuresList()
    {
        var res = new List<ConnectedStructure>();
        foreach (var connector in _connectors)
        {
            if (connector._currentStructure != null)
                res.Add(connector._currentStructure);
        }
        return res;
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

    [ServerRpc(RequireOwnership = false)]
    private async void ConnectBlocks()
    {
        if(PlayerNetCode.Singleton == null || !PlayerNetCode.Singleton.IsServer) return;
        await Task.Delay(100);
        ConnectedStructure currentStructure = null;
        List<ConnectedStructure> structures = new List<ConnectedStructure>();
        structures = GetAddedStructures(structures, GetRelativeStructuresList());
        
        int i = 0;
        
        if (structures.Count == 0)
            currentStructure = GetInstantiatedStructure();
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
        }
        
        SetNewStructure(currentStructure);
    }
    
}