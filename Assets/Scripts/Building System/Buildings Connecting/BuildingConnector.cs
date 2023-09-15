using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BuildingConnector : MonoBehaviour
{
    [SerializeField] private ConnectedStructure _structurePrefab;
    [SerializeField] private BuildingBlock _buildingBlock;

    private ConnectedStructure _currentStructure;

    private List<BuildingConnector> _connectors = new List<BuildingConnector>();

    public ConnectedStructure GetInstantiatedStructure()
    {
        _currentStructure = Instantiate(_structurePrefab, transform.position, Quaternion.identity);
        _buildingBlock.transform.SetParent(_currentStructure.transform);
        return _currentStructure;
    }
    
    public void SetNewStructure(ConnectedStructure structure)
    {
        structure.Blocks.Add(_buildingBlock);
        _currentStructure = structure;
        _buildingBlock.transform.SetParent(_currentStructure.transform);
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
}