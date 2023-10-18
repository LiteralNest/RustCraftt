using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LaunchVoiceUI : MonoBehaviour
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private Toggle _voiceToggle;

    private void Awake()
    {
        _hostButton.onClick.AddListener(() =>
        {
            // VivoxPlayer vivoxPlayer = FindObjectOfType<VivoxPlayer>();
            // if (vivoxPlayer != null)
            // {
            //     vivoxPlayer.SignIntoVivox();
            // }
            NetworkManager.Singleton.StartHost();

        });

        _clientButton.onClick.AddListener(() =>
        {
            // VivoxPlayer vivoxPlayer = FindObjectOfType<VivoxPlayer>();
            // if (vivoxPlayer != null)
            // {
            //     vivoxPlayer.SignIntoVivox();
            // }
            NetworkManager.Singleton.StartClient();
        });

        _voiceToggle.onValueChanged.AddListener(delegate { VivoxToggle(_voiceToggle); });

    }

    void VivoxToggle(Toggle voiceToggle)
    {
        Debug.Log("Voice " + voiceToggle.isOn);
    }
}
