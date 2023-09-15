using System.Collections.Generic;
using UnityEngine;

public class BuildingBluePrint : BluePrint
{
  
    private List<GameObject> _triggeredObjects = new List<GameObject>();

    private void OnEnable()
        => GlobalEventsContainer.InventoryDataChanged += CheckForAvailable;
    
    private void OnDisable()
        => GlobalEventsContainer.InventoryDataChanged -= CheckForAvailable;
    
    private void Start()
    {
        CheckForAvailable();
    }

    public override void CheckForAvailable()
    {
        if (_triggeredObjects.Count != 0)
        {
            CanBePlaced = false;
            DisplayRenderers();
            return;
        }
        CanBePlaced = InventorySlotsContainer.singleton.ItemsAvaliable(TargetBuildingStructure.GetPlacingRemovingCells());
        DisplayRenderers();
    }

    public override void Place()
    {
        BuildingsNetworkingSpawner.singleton.SpawnPrefServerRpc(TargetBuildingStructure.Id, transform.position, transform.rotation);
    }

    public override void TriggerEntered(Collider other)
    {
        if (other.gameObject.tag == "Ground") return;
        _triggeredObjects.Add(other.gameObject);
        CheckForAvailable();
    }

    public override void TriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground") return;
        _triggeredObjects.Remove(other.gameObject);
        CheckForAvailable();
    }
}