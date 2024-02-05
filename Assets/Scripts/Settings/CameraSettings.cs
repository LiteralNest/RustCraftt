using UnityEngine;

namespace Settings
{
    public class CameraSettings : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = GetComponent<Camera>();

            if (_mainCamera == null)
            {
                Debug.LogError("Camera component not found on the GameObject.");
                return;
            }

            ApplyCameraSettings();
        }


        private void ApplyCameraSettings()
        {
            _mainCamera.farClipPlane = GlobalValues.CameraFarDistance;
            _mainCamera.fieldOfView = GlobalValues.CameraFOV;
        }
    }
}