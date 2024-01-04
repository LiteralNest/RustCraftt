using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    private void Start()
    {
#if UNITY_SERVER
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount  = 0;
#else
        Application.targetFrameRate = 120;
#endif
    }
}
