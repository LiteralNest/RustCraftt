using Events;
using UnityEngine;

namespace Map
{
    public class MapHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _map;
        [SerializeField] private GameObject _mapCamera;

        private void OnEnable()
            => GlobalEventsContainer.OnMapOpened += Open;

        private void OnDisable()
            => GlobalEventsContainer.OnMapOpened -= Open;

        public void Open()
        {
            GlobalEventsContainer.OnMainHudHandle?.Invoke(false);
            HandleObjects(true);
        }

        public void Close()
        {
            GlobalEventsContainer.OnMainHudHandle?.Invoke(true);
            HandleObjects(false);
        }

        private void HandleObjects(bool value)
        {
            _map.SetActive(value);
            _mapCamera.SetActive(value);
        }
    }
}