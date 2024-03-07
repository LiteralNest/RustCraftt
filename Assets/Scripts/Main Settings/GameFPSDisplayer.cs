using Settings;
using TMPro;
using UnityEngine;

namespace Main_Settings
{
    public class GameFPSDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _displayText;
        private float _deltaTime;


        private void Start()
        {
            var settings = SettingsContainer.Singleton;
            if(settings == null) return;
            gameObject.SetActive(settings.EnableFPSCounter);
        }

        private void Update () 
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            float fps = 1.0f / _deltaTime;
            _displayText.text = "FPS: " + Mathf.Ceil (fps);
        }
    }
}