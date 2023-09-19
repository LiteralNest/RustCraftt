using System.Collections.Generic;
using UnityEngine;

public class BuildingBluePrint : BluePrint
{
    private List<GameObject> _triggeredObjects = new List<GameObject>();

    private void OnEnable()
        => GlobalEventsContainer.InventoryDataChanged += CheckForAvailable;

    private void OnDisable()
        => GlobalEventsContainer.InventoryDataChanged -= CheckForAvailable;

    private void Update()
    {
        CheckForAvailable();
    }

    public override void CheckForAvailable()
    {
        foreach (var cell in BluePrintCells)
            cell.SetEnoughMaterials(
                InventorySlotsContainer.singleton.ItemsAvaliable(TargetBuildingStructure.GetPlacingRemovingCells()));
    }

    public override void Place()
    {
        foreach (var cell in BluePrintCells)
            cell.TryPlace();
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