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

        private void Awake()
            => Singleton = this;

        public void DisplayRadiationAlert(int value, bool shouldDisplay = true)
        {
            _radiationAlert.gameObject.SetActive(shouldDisplay);
            _radiationAlert.Init(value);
        }

        public void DisplayTooHotAlert(int value, bool shouldDisplay = true)
        {
            _tooHotAlert.gameObject.SetActive(shouldDisplay);
            _tooHotAlert.Init(value);
        }

        public void DisplayTooColdAlert(int value, bool shouldDisplay = true)
        {
            _tooColdAlert.gameObject.SetActive(shouldDisplay);
            _tooColdAlert.Init(value);
        }

        public void DisplayComfortAlert(bool value)
            => _comfortAlert.gameObject.SetActive(value);
        
        public void DisplayPoisonAlert(bool value)
            => _poisonAlert.gameObject.SetActive(value);

        public void DisplayBleedAlert(bool value)
            => _bleedAlert.gameObject.SetActive(value);
    }
}