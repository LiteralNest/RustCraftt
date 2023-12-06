using Alerts_System.Alerts;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerEffectsHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Comfort")) return;
            AlertsDisplayer.Singleton.DisplayComfortAlert(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag("Comfort")) return;
            AlertsDisplayer.Singleton.DisplayComfortAlert(false);
        }
    }
}
