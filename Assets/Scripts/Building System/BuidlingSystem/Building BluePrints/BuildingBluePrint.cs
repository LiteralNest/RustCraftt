using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBluePrint : BluePrint
{
    [Header("Inventory Materials")]
    [SerializeField] protected List<InventoryCell> _neededCellsForPlace = new List<InventoryCell>();

    private void OnEnable()
        => GlobalEventsContainer.InventoryDataChanged += CheckForAvailable;
    
    private void OnDisable()
        => GlobalEventsContainer.InventoryDataChanged -= CheckForAvailable;

    protected void Init()
        => CheckForAvailable();
}