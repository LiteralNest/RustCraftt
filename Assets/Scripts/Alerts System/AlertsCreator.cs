using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertsCreator : MonoBehaviour
{
    [SerializeField] private AddingItemAlertDisplayer _addingItemAlertPrefab;
    [SerializeField] private RemovingAlertDisplayer _removingAlertDisplayer;
    [SerializeField] private Transform _placeForAlerts;

    private void OnEnable()
    {
        GlobalEventsContainer.InventoryItemAdded += CreateAddingItemAlert;
        GlobalEventsContainer.InventoryItemRemoved += CreateRemovingtemAlert;
    }
    
    private void OnDisable()
    {
        GlobalEventsContainer.InventoryItemAdded -= CreateAddingItemAlert;
        GlobalEventsContainer.InventoryItemRemoved -= CreateRemovingtemAlert;
    }

    private void CreateAddingItemAlert(InventoryCell inventoryCell)
    {
        var instance = Instantiate(_addingItemAlertPrefab, _placeForAlerts);
        instance.Init(inventoryCell);
    }

    private void CreateRemovingtemAlert(InventoryCell inventoryCell)
    {
        var instance = Instantiate(_removingAlertDisplayer, _placeForAlerts);
        instance.Init(inventoryCell);
    }
}
