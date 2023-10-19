using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LaunchVoiceUI : MonoBehaviour
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private Toggle _voiceToggle;
    [SerializeField] private Button _speakButton;

    private VivoxPlayer _vivox;
    private void Awake()
    {
        _vivox = FindObjectOfType<VivoxPlayer>();
        
        _hostButton.onClick.AddListener(() =>
        {
            _vivox.SignIntoVivox();
            SwitchOffUi();
        });

        _clientButton.onClick.AddListener(() =>
        {
            _vivox.SignIntoVivox();
            SwitchOffUi();
        });

        _speakButton.onClick.AddListener(() =>
        {
            _vivox.ToggleMicrophone(_speakButton.interactable);
        });
        _voiceToggle.onValueChanged.AddListener(delegate { VivoxToggle(_voiceToggle); });

    }

    private void SwitchOffUi()
    {
        _hostButton.gameObject.SetActive(false);
        _clientButton.gameObject.SetActive(false);
        _speakButton.gameObject.SetActive(true);
    }
    private void VivoxToggle(Toggle voiceToggle)
    {
        Debug.Log("Voice " + voiceToggle.isOn);
    }
}
