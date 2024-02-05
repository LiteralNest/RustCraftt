using AlertsSystem.AlertTypes.Alerts;
using AlertsSystem.AlertTypes.Item_Alert;
using UnityEngine;

namespace AlertsSystem
{
    public class CraftingAlertsView : MonoBehaviour
    {
        [SerializeField] private AddingItemAlertDisplayer _addingItemAlertPrefab;
        [SerializeField] private RemovingAlertDisplayer _removingAlertDisplayer;
        [SerializeField] private CreatingQueueAlertDisplayer _creatingQueueAlertDisplayer;
        [SerializeField] private Transform _placeForAlerts;

        private void OnEnable()
        {
            AlertEventsContainer.OnInventoryItemAdded += CreateAddingItemAlert;
            AlertEventsContainer.OnInventoryItemRemoved += CreateRemovingtemAlert;
            AlertEventsContainer.OnCreatingQueueAlertDataChanged += DisplayCreatingQueueAlert;
        }
    
        private void OnDisable()
        {
            AlertEventsContainer.OnInventoryItemAdded -= CreateAddingItemAlert;
            AlertEventsContainer.OnInventoryItemRemoved -= CreateRemovingtemAlert;
            AlertEventsContainer.OnCreatingQueueAlertDataChanged -= DisplayCreatingQueueAlert;
        }

        private void CreateAddingItemAlert(string itemName, int count)
        {
            var instance = Instantiate(_addingItemAlertPrefab, _placeForAlerts);
            instance.Init(itemName, count);
        }

        private void CreateRemovingtemAlert(string itemName, int count)
        {
            var instance = Instantiate(_removingAlertDisplayer, _placeForAlerts);
            instance.Init(itemName, count);
        }
        
        public void DisplayCreatingQueueAlert(string name, int count, int time)
        {
            _creatingQueueAlertDisplayer.gameObject.SetActive(name != null);
            _creatingQueueAlertDisplayer.Init(name, count, time);
        }
    }
}
