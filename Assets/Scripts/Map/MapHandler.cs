using Events;
using UnityEngine;

namespace Map
{
    public class MapHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _map;

        private void OnEnable()
            => GlobalEventsContainer.OnMapOpened += Open;
        
        private void OnDisable()
            => GlobalEventsContainer.OnMapOpened -= Open;

        public void Open()
        {
            _map.SetActive(true);
        }

        public void Close()
        {
            _map.SetActive(false);
        }
    }
}