using TMPro;
using UI.DeathScreen;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RespawnSystem.SleepingBag
{
    public class SleepingBagMapPointView : MonoBehaviour, IPointerDownHandler
    {
        [Header("UI")] [SerializeField] private TMP_Text _remainingTimeText;
        [SerializeField] private GameObject _reloadingPanel;
        [SerializeField] private SleepingBag _sleepingBag;
        private bool _canRespawn;
        
        private void OnEnable()
        {
            if (_sleepingBag == null) return;
            UpdateRemainingTime(_sleepingBag.ReloadTime.Value); 
        }
        
        private void Start()
        {
            UpdateRemainingTime(_sleepingBag.ReloadTime.Value);

            _sleepingBag.ReloadTime.OnValueChanged += (int oldValue, int newValue) =>
            {
                UpdateRemainingTime(newValue);
            };
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Clicked");
            if(!_canRespawn) return;
            DeathScreenUI.Singleton.RespawnInCoordinates(transform.position);
            _sleepingBag.RespawnPlayerServerRpc();
        }
        
        private void UpdateRemainingTime(int value)
        {
            _canRespawn = value <= 0;
            _reloadingPanel.SetActive(!_canRespawn);
            _remainingTimeText.text = value.ToString() + "s";
        }
    }
}