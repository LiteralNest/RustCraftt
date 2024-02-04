using TMPro;
using UnityEngine;

namespace AlertsSystem.AlertTypes.Alerts
{
    public class TemperatureAlertDisplayer : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _degreeText;

        public void Init(int degreeValue)
        {
            _degreeText.text = degreeValue + "°c"; ;
        }
    }
}
