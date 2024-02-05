using TMPro;
using UnityEngine;

namespace AlertsSystem.AlertTypes.Alerts
{
    public class WorkBenchAlert : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _degreeText;

        public void Init(int workBenchLevel)
        {
            _degreeText.text = "WorkBench Level " + workBenchLevel;
        }
    }
}
