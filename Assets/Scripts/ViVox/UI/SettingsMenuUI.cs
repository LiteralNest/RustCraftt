using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
  [SerializeField] private TMP_Dropdown _qualityDropdown;
  [SerializeField] private Slider _farDistanceSlider;
  [SerializeField] private Slider _fovSlider;
  [SerializeField] private Toggle _shadowsToggle;
  [SerializeField] private Toggle _grassToggle;
  [SerializeField] private Slider _fpsSlider;
  [SerializeField] private Slider _sensitivitySlider;

  [Header("Info")] 
  [SerializeField] private TextMeshProUGUI _renderSliderInfo;
  [SerializeField] private TextMeshProUGUI _fovSliderInfo;
  [SerializeField] private TextMeshProUGUI _sensSliderInfo;
  [SerializeField] private TextMeshProUGUI _fpsSliderInfo;
  

   private void Start()
   {
      _qualityDropdown.value = GlobalValues.GraphicsQualityIndex;
      _farDistanceSlider.value = GlobalValues.CameraFarDistance;
      _fovSlider.value = GlobalValues.CameraFOV;
      _shadowsToggle.isOn = GlobalValues.EnableShadows;
      _grassToggle.isOn = GlobalValues.EnableGrass;
      _fpsSlider.value = GlobalValues.FixedFPS;
      _sensitivitySlider.value = GlobalValues.Sensitivity;
      
      UpdateSliderValueText(_renderSliderInfo, GlobalValues.CameraFarDistance);
      UpdateSliderValueText(_fovSliderInfo, GlobalValues.CameraFOV);
      UpdateSliderValueText(_sensSliderInfo, GlobalValues.Sensitivity);
   }

   private void OnEnable()
   {
      _farDistanceSlider.onValueChanged.AddListener(UpdateRenderSliderText);
      _fovSlider.onValueChanged.AddListener(UpdateFOVSliderText);
      _sensitivitySlider.onValueChanged.AddListener(UpdateSensitivitySliderText);
      _fpsSlider.onValueChanged.AddListener(UpdateFixedFPS);
   }

   private void OnDisable()
   {
      _farDistanceSlider.onValueChanged.RemoveListener(UpdateRenderSliderText);
      _fovSlider.onValueChanged.RemoveListener(UpdateFOVSliderText);
      _sensitivitySlider.onValueChanged.RemoveListener(UpdateSensitivitySliderText);
      _fpsSlider.onValueChanged.RemoveListener(UpdateFixedFPS);
   }
   
   public void SaveGraphicsSettings()
   {
      GlobalValues.GraphicsQualityIndex = _qualityDropdown.value;
      GlobalValues.CameraFarDistance = _farDistanceSlider.value;
      GlobalValues.CameraFOV = _fovSlider.value;
      GlobalValues.EnableShadows = _shadowsToggle;
      GlobalValues.EnableGrass = _grassToggle;
      GlobalValues.FixedFPS = Mathf.RoundToInt(_fpsSlider.value);
   }
   public void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);

   private void UpdateFixedFPS(float value)
   {
      int newFPS = Mathf.RoundToInt(value);
      GlobalValues.FixedFPS = newFPS;
      Application.targetFrameRate = newFPS;
      UpdateSliderValueText(_fpsSliderInfo, value);
   }
   
   private void UpdateSliderValueText(TMP_Text valueText, float value)
   {
      if (valueText != null)
      {
         valueText.text = $"{value}";
      }
   }

   public void UpdateRenderSliderText(float value) => UpdateSliderValueText(_renderSliderInfo, value);

   public void UpdateFOVSliderText(float value) => UpdateSliderValueText(_fovSliderInfo, value);

   public void UpdateSensitivitySliderText(float value) => UpdateSliderValueText(_sensSliderInfo, value);
}
