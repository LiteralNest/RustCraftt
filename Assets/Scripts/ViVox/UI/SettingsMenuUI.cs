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
        [SerializeField] private Slider _fovSlider;
        [SerializeField] private Slider _fpsSlider;
        [SerializeField] private Slider _sensitivitySlider;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Toggle _shadowsToggle;
        [SerializeField] private Toggle _grassToggle;
        [SerializeField] private Toggle _fpsCounterToggle;
        [SerializeField] private TextMeshProUGUI _renderSliderInfo;
        [SerializeField] private TextMeshProUGUI _fovSliderInfo;
        [SerializeField] private TextMeshProUGUI _sensSliderInfo;
        [SerializeField] private TextMeshProUGUI _fpsSliderInfo;
        [SerializeField] private TextMeshProUGUI _volumeSliderInfo;

#if !UNITY_SERVER
        private void Start()
        {
            SetStartVolume();
            InitializeUIValues();
        }

        private void OnEnable()
        {
            AddListeners();
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
            _fovSlider.onValueChanged.AddListener(UpdateFOVSliderText);
            _fpsSlider.onValueChanged.AddListener(UpdateFixedFPS);
            _volumeSlider.onValueChanged.AddListener(UpdateVolume);
            _sensitivitySlider.onValueChanged.AddListener(UpdateSensitivitySliderText);
        }

        private void RemoveListeners()
        {
            _farDistanceSlider.onValueChanged.RemoveListener(UpdateRenderSliderText);
            _fovSlider.onValueChanged.RemoveListener(UpdateFOVSliderText);
            _fpsSlider.onValueChanged.RemoveListener(UpdateFixedFPS);
            _volumeSlider.onValueChanged.RemoveListener(UpdateVolume);
            _sensitivitySlider.onValueChanged.RemoveListener(UpdateSensitivitySliderText);
        }

        private void InitializeUIValues()
        {
            _qualityDropdown.value = GlobalValues.GraphicsQualityIndex;
            _farDistanceSlider.value = GlobalValues.CameraFarDistance;
            _fovSlider.value = GlobalValues.CameraFOV;
            _shadowsToggle.isOn = GlobalValues.EnableShadows;
            _grassToggle.isOn = GlobalValues.EnableGrass;
            _fpsSlider.value = GlobalValues.FixedFPS;
            _fpsCounterToggle.isOn = GlobalValues.EnableFPSCounter;
            _sensitivitySlider.value = GlobalValues.Sensitivity;

            UpdateRenderSliderText(GlobalValues.CameraFarDistance);
            UpdateFOVSliderText(GlobalValues.CameraFOV);
            UpdateSensitivitySliderText(GlobalValues.Sensitivity);
        }

        private void UpdateGlobalValues()
        {
            GlobalValues.GraphicsQualityIndex = _qualityDropdown.value;
            GlobalValues.CameraFarDistance = _farDistanceSlider.value;
            GlobalValues.CameraFOV = _fovSlider.value;
            GlobalValues.EnableShadows = _shadowsToggle.isOn;
            GlobalValues.EnableGrass = _grassToggle.isOn;
            GlobalValues.EnableFPSCounter = _fpsCounterToggle.isOn;
            GlobalValues.FixedFPS = Mathf.RoundToInt(_fpsSlider.value);
            GlobalValues.Volume = _volumeSlider.value;
        }

        private void UpdateFixedFPS(float value)
        {
            int newFPS = Mathf.RoundToInt(value);
            GlobalValues.FixedFPS = newFPS;
            Application.targetFrameRate = newFPS;
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

        private void UpdateFOVSliderText(float value) => UpdateSliderValueText(_fovSliderInfo, value);

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
            GlobalValues.Volume = volume;
        }
#endif
    }
}
