using System;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ViVox.UI
{
    public class SettingsMenuUI : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private TMP_Dropdown _qualityDropdown;
        [SerializeField] private Slider _farDistanceSlider;
        [SerializeField] private Slider _fpsSlider;
        [SerializeField] private Slider _sensitivitySlider;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Toggle _grassToggle;
        [SerializeField] private Toggle _fpsCounterToggle;
        [SerializeField] private TextMeshProUGUI _renderSliderInfo;
        [SerializeField] private TextMeshProUGUI _sensSliderInfo;
        [SerializeField] private TextMeshProUGUI _fpsSliderInfo;
        [SerializeField] private TextMeshProUGUI _volumeSliderInfo;

#if !UNITY_SERVER

        private void Start()
        {
            SetStartVolume();
        
        }

        private void OnEnable()
        {
            AddListeners();
            InitializeUIValues();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        public void SaveSettings()
        {
            UpdateGlobalValues();
        }

        public void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);

        private void SetStartVolume()
        {
            var initialVolume = _volumeSlider.value;
            _audioMixer.SetFloat("Main", initialVolume);
        }

        private void AddListeners()
        {
            _farDistanceSlider.onValueChanged.AddListener(UpdateRenderSliderText);
            _fpsSlider.onValueChanged.AddListener(UpdateFixedFPS);
            _volumeSlider.onValueChanged.AddListener(UpdateVolume);
            _sensitivitySlider.onValueChanged.AddListener(UpdateSensitivitySliderText);
        }

        private void RemoveListeners()
        {
            _farDistanceSlider.onValueChanged.RemoveListener(UpdateRenderSliderText);
            _fpsSlider.onValueChanged.RemoveListener(UpdateFixedFPS);
            _volumeSlider.onValueChanged.RemoveListener(UpdateVolume);
            _sensitivitySlider.onValueChanged.RemoveListener(UpdateSensitivitySliderText);
        }

        private void InitializeUIValues()
        {
            var settings = SettingsContainer.Singleton;
            if(settings == null) return;
            
            _qualityDropdown.value =  QualitySettings.GetQualityLevel();
            _farDistanceSlider.value = settings.CameraFarDistance;
            _grassToggle.isOn = settings.EnableGrass;
            _fpsSlider.value = Application.targetFrameRate;
            _fpsSliderInfo.text = $"{Application.targetFrameRate}";
            _fpsCounterToggle.isOn = settings.EnableFPSCounter;
            _sensitivitySlider.value = settings.Sensitivity;

            UpdateRenderSliderText(settings.CameraFarDistance);
            UpdateSensitivitySliderText(settings.Sensitivity);
        }

        private void UpdateGlobalValues()
        {
            var settings = SettingsContainer.Singleton;
            if(settings == null) return;
            
            int newFPS = Mathf.RoundToInt(_fpsSlider.value);
            Application.targetFrameRate = newFPS;
            QualitySettings.SetQualityLevel(_qualityDropdown.value);
            
            settings.Volume = _volumeSlider.value;
            settings.EnableGrass = _grassToggle.isOn;
            settings.CameraFarDistance = Mathf.RoundToInt(_farDistanceSlider.value);
            settings.EnableFPSCounter = _fpsCounterToggle.isOn;
            settings.Sensitivity = _sensitivitySlider.value;
            settings.Save();
        }

        private void UpdateFixedFPS(float value)
        {
           
            UpdateSliderValueText(_fpsSliderInfo, value);
        }

        private void UpdateSliderValueText(TextMeshProUGUI valueText, float value)
        {
            if (valueText != null)
            {
                valueText.text = $"{value}";
            }
        }

        private void UpdateRenderSliderText(float value) => UpdateSliderValueText(_renderSliderInfo, value);

        private void UpdateSensitivitySliderText(float value)
        {
            _sensSliderInfo.text = value.ToString("0.0");
        }

        private void UpdateVolume(float volume)
        {
            var dbVolume = Mathf.Lerp(-20f, 20f, volume);
            _audioMixer.SetFloat("Main", dbVolume);
            var displayVolume = volume * 100f;
            _volumeSliderInfo.SetText($"{displayVolume:N0}");
        }
#endif
    }
}
