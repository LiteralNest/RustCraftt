using System.Threading.Tasks;
using Building_System.Buildings_Connecting;
using Tool_Clipboard;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class ToolClipboardConnector : MonoBehaviour
{
    [SerializeField] private ToolClipboard _targetClipBoard;
    private BuildingConnector _buildingConnector;

    private async void Start()
    {
        await Task.Delay(1000);
        ConnectToStructure();
    }
    
    private void ConnectToStructure()
    {
        if(!_buildingConnector.CurrentStructure.TargetClipBoards.Contains(_targetClipBoard)) return;
        _buildingConnector.CurrentStructure.TargetClipBoards.Add(_targetClipBoard);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out BuildingConnector connector)) return;
        _buildingConnector = connector;
    }
}