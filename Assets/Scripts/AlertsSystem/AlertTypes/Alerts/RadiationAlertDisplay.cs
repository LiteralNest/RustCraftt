using TMPro;
using UnityEngine;

namespace AlertsSystem.AlertTypes.Alerts
{
    public class RadiationAlertDisplay : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _valueText; 

        public void Init(int value)
        {
            _valueText.text = value + "";
        }
    }
}
