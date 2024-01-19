using Unity.Netcode;
using UnityEngine;

namespace DayTime
{
    public class SkyBoxView : NetworkBehaviour
    {
        private static readonly int Rotation = Shader.PropertyToID("_Rotation");
        private static readonly int Exposure = Shader.PropertyToID("_Exposure");

        [Header("Attached Components")] [SerializeField]
        private Material _dayMaterial;

        [SerializeField] private Material _nightMaterial;
        [SerializeField] private Light _directionalLight;

        public NetworkVariable<int> CurrentPhase = new();
        public NetworkVariable<float> RotationAmount = new();
        public NetworkVariable<float> ExposureAmount = new();
        public NetworkVariable<float> LightIntensity = new();

        public override void OnNetworkSpawn()
        {
            HandleCurrentPhase(CurrentPhase.Value);
            HandleRotationAmount(RotationAmount.Value);
            HandleExposureAmount(ExposureAmount.Value);
            HandleLightIntensity(LightIntensity.Value);
            
            base.OnNetworkSpawn();
            CurrentPhase.OnValueChanged += (int oldValue, int newValue) => HandleCurrentPhase(newValue);
            RotationAmount.OnValueChanged += (float oldValue, float newValue) => HandleRotationAmount(newValue);
            ExposureAmount.OnValueChanged += (float oldValue, float newValue) => RenderSettings.skybox.SetFloat(Exposure, newValue);
            LightIntensity.OnValueChanged += (float oldValue, float newValue) => _directionalLight.intensity = newValue;
        }

        private void HandleCurrentPhase(int value)
        {
            switch (value)
            {
                case 0:
                    RenderSettings.skybox = _dayMaterial;
                    break;
                case 1:
                    RenderSettings.skybox = _nightMaterial;
                    break;
            }
        }

        private void HandleRotationAmount(float value)
            => RenderSettings.skybox.SetFloat(Rotation, value);

        private void HandleExposureAmount(float value)
            => RenderSettings.skybox.SetFloat(Exposure, value);

        private void HandleLightIntensity(float value)
            => _directionalLight.intensity = value;
    }
}