using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LaunchVoiceScreenUI : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private TMP_InputField _displayName;

    //Remove
    [SerializeField] private Button _speakButton;

    private VivoxConnector _vivox;
    private VivoxVoiceManager _vvm;
    private int _defaultMaxStringLength = 9;

    private void Awake()
    {
        _vivox = FindObjectOfType<VivoxConnector>();
        _displayName.text = Environment.MachineName.Substring(0, Math.Min(_defaultMaxStringLength, Environment.MachineName.Length));
        

#if !(UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_STADIA)
        DisplayNameInput.interactable = false;
#else
        // _displayName.onEndEdit.AddListener((string text) => _vivox.LoginIntoViVox(_displayName, _defaultMaxStringLength));
#endif
        _startButton.onClick.AddListener(UserLoggedIn);
        
    }

    private void UserLoggedIn()
    {
        _vivox.SignIntoViVox(_displayName.text);
        _displayName.text = _vvm.LoginSession.Key.DisplayName;
        SwitchOffUi();
    }


    private void SwitchOffUi()
    {
        gameObject.SetActive(false);
        _speakButton.gameObject.SetActive(true);
    }
    

}