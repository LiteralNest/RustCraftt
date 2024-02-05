using Events;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerCamerasDisabler : MonoBehaviour
    {
        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private GameObject _inventotyCamera;

        private void OnEnable()
            => GlobalEventsContainer.OnMainHudHandle += HandleCameras;
        
        private void OnDisable()
            => GlobalEventsContainer.OnMainHudHandle -= HandleCameras;
        
        private void HandleCameras(bool value)
        {
            _mainCamera.SetActive(value);
            _inventotyCamera.SetActive(value);
        }
    }
}