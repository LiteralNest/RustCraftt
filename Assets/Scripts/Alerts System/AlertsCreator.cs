using Alerts_System.Alerts.Item_Alert;
using Events;
using UnityEngine;

namespace Alerts_System
{
    public class AlertsCreator : MonoBehaviour
    {
        [SerializeField] private AddingItemAlertDisplayer _addingItemAlertPrefab;
        [SerializeField] private RemovingAlertDisplayer _removingAlertDisplayer;
        [SerializeField] private Transform _placeForAlerts;

        private void OnEnable()
        {
            GlobalEventsContainer.OnInventoryItemAdded += CreateAddingItemAlert;
            GlobalEventsContainer.OnInventoryItemRemoved += CreateRemovingtemAlert;
        }
    
        private void OnDisable()
        {
            GlobalEventsContainer.OnInventoryItemAdded -= CreateAddingItemAlert;
            GlobalEventsContainer.OnInventoryItemRemoved -= CreateRemovingtemAlert;
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
}
