using TMPro;
using UnityEngine;

namespace AlertsSystem.AlertTypes.Alerts
{
    public class CreatingQueueAlertDisplayer : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _timeText;

        public void Init(string itemName, int count, int time)
        {
            _titleText.text = itemName + "(" + count + ")";
            _timeText.text = time + "s";
        }
    }
}
