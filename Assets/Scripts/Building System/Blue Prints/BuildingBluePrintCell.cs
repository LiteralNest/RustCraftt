using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class BuildingBluePrintCell : MonoBehaviour
{
    [Header("Start Init")]
    [SerializeField] private BluePrint _bluePrint;
    [SerializeField] private BuildingStructure _targetBuildingStructure;
    [Header("Materials")] [SerializeField] private Material _negativeMaterial;
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Renderer _renderer;

    private List<GameObject> _triggeredObjects = new List<GameObject>();
    public bool CanBePlaced { get; private set; }

    private bool _enoughMaterials;

    private void OnEnable()
        => GlobalEventsContainer.InventoryDataChanged += CheckEnoughMaterials;

    private void OnDisable()
        => GlobalEventsContainer.InventoryDataChanged -= CheckEnoughMaterials;

    private void Start()
        => CheckEnoughMaterials();

    private void SetMaterial(Material material)
        => _renderer.material = material;

    private void SetCanBePlaced(bool value)
    {
        CanBePlaced = value;
        if (!_enoughMaterials || !CanBePlaced)
            SetMaterial(_negativeMaterial);
        else
            SetMaterial(_normalMaterial);
    }

    public void CheckForAvailable()
    {
        if (!_enoughMaterials || _triggeredObjects.Count > 0 || _bluePrint.OnFrontOfPlayer)
        {
            SetCanBePlaced(false);
            return;
        }

        SetCanBePlaced(true);
    }

    public void CheckEnoughMaterials()
    {
        _enoughMaterials = InventorySlotsContainer.singleton.EnoughMaterials(_targetBuildingStructure.GetPlacingRemovingCells());
    }

    public void TryPlace()
    {
        if (!CanBePlaced) return;
        foreach (var cell in _targetBuildingStructure.GetPlacingRemovingCells())
            InventorySlotsContainer.singleton.RemoveItemFromDesiredSlot(cell.Item, cell.Count);
        BuildingsNetworkingSpawner.singleton.SpawnPrefServerRpc(_targetBuildingStructure.Id, transform.position,
            transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ConnectingPoint")) return;
        if (_triggeredObjects.Contains(other.gameObject)) return;
        _triggeredObjects.Add(other.gameObject);
        CheckForAvailable();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ConnectingPoint")) return;
        if (!_triggeredObjects.Contains(other.gameObject)) return;
        _triggeredObjects.Remove(other.gameObject);
        CheckForAvailable();
    }
}