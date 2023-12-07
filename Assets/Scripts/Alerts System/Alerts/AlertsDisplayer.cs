using Alerts_System.Alerts.Alerts;
using UnityEngine;

namespace Alerts_System.Alerts
{
    public class AlertsDisplayer : MonoBehaviour
    {
        public static AlertsDisplayer Singleton { get; set; }

        [SerializeField] private RadiationAlertDisplay _radiationAlert;
        [SerializeField] private TemperatureAlertDisplayer _tooHotAlert;
        [SerializeField] private TemperatureAlertDisplayer _tooColdAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _poisonAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _comfortAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _bleedAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _starvingAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _dehydratedAlert;
        [SerializeField] private WorkBenchAlert _workBenchAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _buildingBlockedAlert;
        [SerializeField] private OtherEffectsAlertDisplayer _buildingUnblockedAlert;

        private void Awake()
            => Singleton = this;

        public void DisplayRadiationAlert(int value, bool shouldDisplay = true)
        {
            if (_radiationAlert == null) return;
            _radiationAlert.gameObject.SetActive(shouldDisplay);
            _radiationAlert.Init(value);
        }

        public void DisplayTooHotAlert(int value, bool shouldDisplay = true)
        {
            if (_tooHotAlert == null) return;
            _tooHotAlert.gameObject.SetActive(shouldDisplay);
            _tooHotAlert.Init(value);
        }

        public void DisplayTooColdAlert(int value, bool shouldDisplay = true)
        {
            if (_tooColdAlert == null) return;
            _tooColdAlert.gameObject.SetActive(shouldDisplay);
            _tooColdAlert.Init(value);
        }

        public void DisplayComfortAlert(bool value)
        {
            if (_comfortAlert == null) return;
            _comfortAlert.gameObject.SetActive(value);
        }

        public void DisplayPoisonAlert(bool value)
        {
            if (_poisonAlert == null) return;
            _poisonAlert.gameObject.SetActive(value);
        }

        public void DisplayBleedAlert(bool value)
        {
            if (_bleedAlert == null) return;
            _bleedAlert.gameObject.SetActive(value);
        }

        public void DisplayStarvingAlert(bool value)
        {
            if (_starvingAlert == null) return;
            _starvingAlert.gameObject.SetActive(value);
        }

        public void DisplayDehydratedAlert(bool value)
            => _dehydratedAlert.gameObject.SetActive(value);

        public void DisplayWorkBenchAlert(int workBenchLevel, bool value)
        {
            _workBenchAlert.gameObject.SetActive(value);
            _workBenchAlert.Init(workBenchLevel);
        }

        public void DisplayBuildingBlockedAlert(bool value)
        {
            if (_buildingBlockedAlert == null) return;
            _buildingBlockedAlert.gameObject.SetActive(value);
        }

        public void DisplayBuildingUnblockedAlert(bool value)
        {
            if (_buildingUnblockedAlert == null) return;
            _buildingUnblockedAlert.gameObject.SetActive(value);
        }
    }
}