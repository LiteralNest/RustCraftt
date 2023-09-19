 using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Serialization;

 [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class BuildingBluePrintCell : MonoBehaviour
{
    [SerializeField] private BuildingStructure _targetBuildingStructure;
    [Header("Materials")] 
    [SerializeField] private Material _negativeMaterial;
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Renderer _renderer;
    
    private List<GameObject> _triggeredObjects = new List<GameObject>();
    private bool _canBePlaced;

    private bool _enoughMaterials;
    
    private void SetMaterial(Material material)
        => _renderer.material = material;

    private void SetCanBePlaced(bool value)
    {
        _canBePlaced = value;
        if(_canBePlaced)
            SetMaterial(_normalMaterial);
        else
            SetMaterial(_negativeMaterial);
    }

    private void CheckForAvailable()
    {
        if (!_enoughMaterials || _triggeredObjects.Count > 0)
        {
            SetCanBePlaced(false);
            return;
        }
        SetCanBePlaced(true);
    }
    
    public void SetEnoughMaterials(bool value)
    {
        _enoughMaterials = value;
        CheckForAvailable();
    }

    public void TryPlace()
    {
        if (!_canBePlaced) return;
        foreach (var cell in _targetBuildingStructure.GetPlacingRemovingCells())
            InventorySlotsContainer.singleton.DeleteSlot(cell.Item, cell.Count);
        BuildingsNetworkingSpawner.singleton.SpawnPrefServerRpc(_targetBuildingStructure.Id, transform.position,
            transform.rotation);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            if(_triggeredObjects.Contains(other.gameObject)) return;
            _triggeredObjects.Add(other.gameObject);
            CheckForAvailable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            if(!_triggeredObjects.Contains(other.gameObject)) return;
            _triggeredObjects.Remove(other.gameObject);
            CheckForAvailable();
        }
    }
}