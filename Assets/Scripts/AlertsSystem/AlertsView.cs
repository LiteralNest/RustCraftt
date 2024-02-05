using AlertsSystem.AlertTypes.Alerts;
using UnityEngine;

namespace AlertsSystem
{
    public class AlertsView : MonoBehaviour
    {
        [SerializeField] private OtherEffectsAlertDisplayer _comfortAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _bleedAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _starvingAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _dehydratedAlert;
        [SerializeField] private WorkBenchAlert _workBenchAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _buildingBlockedAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _buildingUnblockedAlert;

        private void OnEnable()
        {
            AlertEventsContainer.OnBleedAlert += DisplayBleedAlert;
            AlertEventsContainer.OnComfortAlert += DisplayComfortAlert;
            AlertEventsContainer.OnDehydratedAlert += DisplayDehydratedAlert;
            AlertEventsContainer.OnStarvingAlert += DisplayStarvingAlert;
            AlertEventsContainer.OnWorkBenchAlert += DisplayWorkBenchAlert;
            AlertEventsContainer.OnBuildingBlockedAlert += DisplayBuildingBlockedAlert;
            AlertEventsContainer.OnBuildingUnblockedAlert += DisplayBuildingUnblockedAlert;
        }
        
        private void OnDisable()
        {
            AlertEventsContainer.OnBleedAlert -= DisplayBleedAlert;
            AlertEventsContainer.OnComfortAlert -= DisplayComfortAlert;
            AlertEventsContainer.OnDehydratedAlert -= DisplayDehydratedAlert;
            AlertEventsContainer.OnStarvingAlert -= DisplayStarvingAlert;
            AlertEventsContainer.OnWorkBenchAlert -= DisplayWorkBenchAlert;
            AlertEventsContainer.OnBuildingBlockedAlert -= DisplayBuildingBlockedAlert;
            AlertEventsContainer.OnBuildingUnblockedAlert -= DisplayBuildingUnblockedAlert;
        }
        
        private void DisplayComfortAlert(bool value)
        {
            if (_comfortAlert == null) return;
            _comfortAlert.gameObject.SetActive(value);
        }
        
        private void DisplayBleedAlert(bool value)
        {
            if (_bleedAlert == null) return;
            _bleedAlert.gameObject.SetActive(value);
        }

        private void DisplayStarvingAlert(bool value)
        {
            if (_starvingAlert == null) return;
            _starvingAlert.gameObject.SetActive(value);
        }

        private void DisplayDehydratedAlert(bool value)
        {
            if(_dehydratedAlert == null) return;
            _dehydratedAlert.gameObject.SetActive(value);
        }

        private void DisplayWorkBenchAlert(int workBenchLevel, bool value)
        {
            if(_workBenchAlert == null) return;
            _workBenchAlert.gameObject.SetActive(value);
            _workBenchAlert.Init(workBenchLevel);
        }

        private void DisplayBuildingBlockedAlert(bool value)
        {
            if (_buildingBlockedAlert == null) return;
            _buildingBlockedAlert.gameObject.SetActive(value);
        }

        private void DisplayBuildingUnblockedAlert(bool value)
        {
            if (_buildingUnblockedAlert == null) return;
            _buildingUnblockedAlert.gameObject.SetActive(value);
        }
    }
}