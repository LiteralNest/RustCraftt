using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class BuildingConnector : MonoBehaviour
{
    [SerializeField] protected ConnectedStructure _structurePrefab;
    
    public ConnectedStructure CurrentStructure => _currentStructure;
    protected ConnectedStructure _currentStructure;

    protected List<BuildingConnector> _connectors = new List<BuildingConnector>();

    private void Start()
    {
        ConnectBlocks();
    }

    #region abstractd

    public abstract void ConnectStructures();

    #endregion
    
    
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
    
    protected List<ConnectedStructure> GetAddedStructures(List<ConnectedStructure> startedStructures,
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
    private void ConnectBlocks()
    {
        ConnectStructures();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ConnectingPoint") || !other.TryGetComponent(out BuildingConnector connector)) return;
        if (_connectors.Contains(connector)) return;
        _connectors.Add(connector);
    }
}