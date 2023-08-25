using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertsCreator : MonoBehaviour
{
    [SerializeField] private AddingItemAlertDisplayer _addingItemAlertPrefab;
    [SerializeField] private Transform _placeForAlerts;

    private void OnEnable()
    {
        GlobalEventsContainer.InventoryItemAdded += CreateItemAlert;
    }
    
    private void OnDisable()
    {
        GlobalEventsContainer.InventoryItemAdded -= CreateItemAlert;
    }

    private void CreateItemAlert(InventoryItem inventoryItem)
    {
        var instance = Instantiate(_addingItemAlertPrefab, _placeForAlerts);
        instance.Init(inventoryItem);
    }
}
