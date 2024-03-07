using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapDisplayUI : MonoBehaviour
    {
    
        [SerializeField] private Camera _mapCamera;

        [SerializeField] private GameObject _show;
        [SerializeField] private GameObject _hide;
        
        [SerializeField] private Button _buttonShow;
        [SerializeField] private Button _buttonHide;

        private void Awake()
        {
            _hide.SetActive(false);
            _mapCamera.enabled = false;
        }

        private void Start()
        {
            _buttonShow.onClick.AddListener(ShowMap);
            _buttonHide.onClick.AddListener(HideMap);
        }

        private void HideMap()
        {
            _show.SetActive(true);
            _hide.SetActive(false);

            _mapCamera.enabled = false;
        }

        private void ShowMap()
        {
            _show.SetActive(false);
            _hide.SetActive(true);

            _mapCamera.enabled = true;
        }
    }
}