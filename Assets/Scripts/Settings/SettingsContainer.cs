using UnityEngine;

namespace Settings
{
    public class SettingsContainer : MonoBehaviour
    {
        public static SettingsContainer Singleton { get; private set; }

        [SerializeField] private bool _enableGrass = true;
        [SerializeField] private int _cameraFarDistance = 100;
        [SerializeField] private int _targetFrameRate = 60;
        [SerializeField] private bool _enableFPSCounter = false;
        [SerializeField] private float _sensitivity = 1;
        [SerializeField] private float _volume = 1;

        public bool EnableGrass
        {
            get => _enableGrass;
            set => _enableGrass = value;
        }
        
        public int FrameRate
        {
            get => _targetFrameRate;
            set => _targetFrameRate = value;
        }

        public int CameraFarDistance
        {
            get => _cameraFarDistance;
            set => _cameraFarDistance = value;
        }

        public bool EnableFPSCounter
        {
            get => _enableFPSCounter;
            set => _enableFPSCounter = value;
        }
        
        
        public float Sensitivity
        {
            get => _sensitivity;
            set => _sensitivity = value;
        }
        
        public float Volume
        {
            get => _volume;
            set => _volume = value;
        }


        private void Awake()
        {
            if (Singleton != null && Singleton != this)
            {
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(gameObject);
            
            var saver = new SettingsDataSaver();
            var data = saver.LoadData();

            if (data == null)
                return;
            
            _enableGrass = data.EnableGrass;
            _cameraFarDistance = data.CameraFarDistance;
            _enableFPSCounter = data.EnableFPSCounter;
            _sensitivity = data.Sensitivity;
            _volume = data.Volume;
        }

        public void Save()
        {
            SettingsDataSaver saver = new SettingsDataSaver();
            saver.SaveData(new SettingsData(_enableGrass, _cameraFarDistance, _enableFPSCounter, _sensitivity, _volume, Application.targetFrameRate));
        }
    }
}