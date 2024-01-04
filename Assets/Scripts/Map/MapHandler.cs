using UnityEngine;

namespace Map
{
    public class MapHandler : MonoBehaviour 
    {
        [SerializeField] private GameObject _map;

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