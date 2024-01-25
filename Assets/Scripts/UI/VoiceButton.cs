using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CustomButton))]
    public class VoiceButton : MonoBehaviour
    {
        private CustomButton _button;

        private void Start()
        {
            _button = GetComponent<CustomButton>();
            _button.PointerDown.AddListener(() => { LobbyScreenUI.Singleton.ToggleMicrophone(true); });
            _button.PointerClicked.AddListener(() => { LobbyScreenUI.Singleton.ToggleMicrophone(false); });
        }
    }
}