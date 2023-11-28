using System.Collections.Generic;
using Inventory_System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class BuildingBluePrintCell : MonoBehaviour
{
    [Header("Start Init")] [SerializeField]
    private BluePrint _bluePrint;

    [SerializeField] private BuildingStructure _targetBuildingStructure;
    [Header("Materials")] [SerializeField] private Material _negativeMaterial;
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private List<Renderer> _renderers;

    private List<GameObject> _triggeredObjects = new List<GameObject>();
    public bool CanBePlaced { get; private set; }

    public bool OnFrontOfPlayer { get; set; }

    private bool _enoughMaterials;

    private void OnEnable()
        => GlobalEventsContainer.InventoryDataChanged += CheckEnoughMaterials;

    private void OnDisable()
        => GlobalEventsContainer.InventoryDataChanged -= CheckEnoughMaterials;

    private void Start()
        => CheckEnoughMaterials();

    private void SetMaterial(Material material)
    {
        foreach (var renderer in _renderers)
            renderer.sharedMaterial = material;
    }

    public virtual bool CanBePlace()
        => !(!_enoughMaterials || _triggeredObjects.Count > 0 || OnFrontOfPlayer);

    private void SetCanBePlaced(bool value)
    {
        CanBePlaced = value;
        if (!CanBePlaced)
            SetMaterial(_negativeMaterial);
        else
            SetMaterial(_normalMaterial);
    }

    public void CheckForAvailable()
    {
        CheckEnoughMaterials();
        if (!CanBePlace())
        {
            SetCanBePlaced(false);
            return;
        }

        SetCanBePlaced(true);
    }

    public void CheckEnoughMaterials()
        => _enoughMaterials = InventoryHelper.EnoughMaterials(_targetBuildingStructure.GetPlacingRemovingCells(),
            InventoryHandler.singleton.CharacterInventory.ItemsNetData);

    public void InitPlacedObject(GameObject target)
        => _bluePrint.InitPlacedObject(target.GetComponent<BuildingStructure>());

    public void TryPlace()
    {
        if (!CanBePlaced) return;

        foreach (var cell in _targetBuildingStructure.GetPlacingRemovingCells())
            InventoryHandler.singleton.CharacterInventory.AddItemToDesiredSlotServerRpc((ushort)cell.Item.Id, (ushort)cell.Count);
        BuildingsNetworkingSpawner.singleton.SpawnPrefServerRpc(_targetBuildingStructure.Id, transform.position,
            transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NoBuild"))
        {
            SetCanBePlaced(false);
            return;
        }

        if (other.CompareTag("ConnectingPoint") || other.CompareTag("ShelfZone") || other.CompareTag("WorkBench")) return;
        if (_triggeredObjects.Contains(other.gameObject)) return;
        _triggeredObjects.Add(other.gameObject);
        CheckForAvailable();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ConnectingPoint") || other.CompareTag("ShelfZone") || other.CompareTag("WorkBench")) return;
        if (!_triggeredObjects.Contains(other.gameObject)) return;
        _triggeredObjects.Remove(other.gameObject);
        CheckForAvailable();
    }
}