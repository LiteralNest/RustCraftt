using UnityEngine;

namespace UI
{
    public class MicrophoneUiDisplayer : MonoBehaviour
    {
        [SerializeField] private GameObject _microphonePanel;

        private void OnEnable()
            => GlobalEventsContainer.OnMicrophoneButtonClicked += HandleMicophonePanle;

        private void OnDisable()
            => GlobalEventsContainer.OnMicrophoneButtonClicked -= HandleMicophonePanle;

        private void HandleMicophonePanle(bool value)
            => _microphonePanel.SetActive(value);
    }
}