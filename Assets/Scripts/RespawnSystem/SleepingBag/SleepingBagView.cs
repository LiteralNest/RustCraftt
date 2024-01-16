using TMPro;
using UI.DeathScreen;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RespawnSystem.SleepingBag
{
    public class SleepingBagView : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private Button _button; 
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _remainingTimeText;
        [SerializeField] private GameObject _reloadingPanel;

        private SleepingBag _sleepingBag;
        public SleepingBag SleepingBag => _sleepingBag;

        private void OnEnable()
        {
            if (_sleepingBag == null) return;
            UpdateName(_sleepingBag.Name.Value.ToString());
            UpdateRemainingTime(_sleepingBag.ReloadTime.Value); 
        }

        public void Init(SleepingBag sleepingBag)
        {
            _sleepingBag = sleepingBag;
            
            UpdateName(sleepingBag.Name.Value.ToString());
            UpdateRemainingTime(sleepingBag.ReloadTime.Value);
            
            _sleepingBag.Name.OnValueChanged += (FixedString64Bytes oldValue, FixedString64Bytes newValue) =>
            {
                UpdateName(newValue.ToString());
            };
            
            _sleepingBag.ReloadTime.OnValueChanged += (int oldValue, int newValue) =>
            {
                UpdateRemainingTime(newValue);
            };
            
            _button.onClick.AddListener(() =>
            {
                DeathScreenUI.Singleton.RespawnInCoordinates(_sleepingBag.transform.position);
                _sleepingBag.RespawnPlayerServerRpc();
            });
        }

        private void UpdateName(string sleepingBagName)
            => _nameText.text = sleepingBagName;

        private void UpdateRemainingTime(int value)
        {
            _reloadingPanel.SetActive(value > 0);
            _remainingTimeText.text = value.ToString() + "s";
        }
    }
}