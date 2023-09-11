using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class BuildingConnector : MonoBehaviour
{
    [SerializeField] private ConnectionsParent _connectionsParentPref;
    [SerializeField] private List<BuildingConnector> _connectedBuildings = new List<BuildingConnector>();

    [Header("In Game Init")] [SerializeField]
    private ConnectionsParent _currentConnectionsParent; 

    private void Start()
        => WaitForConnecting();

    private void ConnectParent(ConnectionsParent parent)
    {
        GetComponent<NetworkObject>().TrySetParent(parent.transform);
        parent.AddConnectedBuilding(this);
    }
    
    private async Task WaitForConnecting()
    {
        await Task.Delay(100);
        if (_connectedBuildings.Count > 0)
            _currentConnectionsParent = _connectedBuildings[0]._currentConnectionsParent;
        else
            _currentConnectionsParent = Instantiate(_connectionsParentPref, transform.position, Quaternion.identity);
        ConnectParent(_currentConnectionsParent);
    }

    public void AddConnectedBuilding(BuildingConnector building)
    {
        if (_connectedBuildings.Contains(building)) return;
        _connectedBuildings.Add(building);
    }

    public void RemoveConnectedBuilding(BuildingConnector building)
    {
        _connectedBuildings.Remove(building);
    }
}