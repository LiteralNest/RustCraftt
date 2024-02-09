using UnityEngine;

namespace Main_Settings
{
    public class GraphicsSettings : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_SERVER
        Application.targetFrameRate = 45;
        QualitySettings.vSyncCount  = 0;
#endif
        }
    }
}
