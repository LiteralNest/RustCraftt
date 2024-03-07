namespace Settings
{
    public class SettingsData
    {
        public bool EnableGrass;
        public int CameraFarDistance;
        public bool EnableFPSCounter;
        public float Sensitivity;
        public float Volume;

        public SettingsData(bool enableGrass, int cameraFarDistance, bool enableFPSCounter, float sensitivity,
            float volume)
        {
            EnableGrass = enableGrass;
            CameraFarDistance = cameraFarDistance;
            EnableFPSCounter = enableFPSCounter;
            Sensitivity = sensitivity;
            Volume = volume;
        }

        public SettingsData()
        {
        }
    }
}