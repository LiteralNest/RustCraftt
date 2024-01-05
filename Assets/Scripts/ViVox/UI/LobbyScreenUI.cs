using System.Linq;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VivoxUnity;

public class LobbyScreenUI : MonoBehaviour
{
    public string LobbyChannelName = "lobbyChannel";
    
    public GameObject LobbyScreen;

    private VivoxVoiceManager _vivoxVoiceManager;
    private EventSystem _evtSystem;
    private bool _isMicrophoneOn;
    private IChannelSession _chan;

    #region Unity Callbacks

    private void Awake()
    {
        _evtSystem = EventSystem.current;
        if (!_evtSystem)
        {
            Debug.LogError("Unable to find EventSystem object.");
        }
        _vivoxVoiceManager = VivoxVoiceManager.Instance;
        _vivoxVoiceManager.OnUserLoggedInEvent += OnUserLoggedIn;
        _vivoxVoiceManager.OnUserLoggedOutEvent += OnUserLoggedOut;

        if (_vivoxVoiceManager.LoginState == LoginState.LoggedIn)
        {
            OnUserLoggedIn();
        }
        else
        {
            OnUserLoggedOut();
        }
    }

    private void OnDestroy()
    {
        _vivoxVoiceManager.OnUserLoggedInEvent -= OnUserLoggedIn;
        _vivoxVoiceManager.OnUserLoggedOutEvent -= OnUserLoggedOut;
    }

    #endregion

    public void ToggleMicrophone(bool isButtonDown)
    {
        if (isButtonDown && !_isMicrophoneOn) 
        {
            GlobalEventsContainer.OnMicrophoneButtonClicked?.Invoke(true);
            _vivoxVoiceManager.AudioInputDevices.Muted = false; 
            _isMicrophoneOn = true;
        }
        else if (!isButtonDown && _isMicrophoneOn) 
        {
            _vivoxVoiceManager.AudioInputDevices.Muted = true;
            _isMicrophoneOn = false;
            GlobalEventsContainer.OnMicrophoneButtonClicked?.Invoke(false);
        }
    }
    
    private void JoinLobbyChannel()
    {
        // Do nothing, participant added will take care of this
        _vivoxVoiceManager.JoinChannel(LobbyChannelName, ChannelType.Positional, VivoxVoiceManager.ChatCapability.TextAndAudio);
        var channelId = new Channel(LobbyChannelName, ChannelType.Positional);
        _chan = _vivoxVoiceManager.LoginSession.GetChannelSession(channelId);
    }
    

    #region Vivox Callbacks
    
    private void OnUserLoggedIn()
    {
        LobbyScreen.SetActive(true);

        var lobbychannel = _vivoxVoiceManager.ActiveChannels.FirstOrDefault(ac => ac.Channel.Name == LobbyChannelName);
        if ((_vivoxVoiceManager && _vivoxVoiceManager.ActiveChannels.Count == 0) 
            || lobbychannel == null)
        {
            JoinLobbyChannel();
            _vivoxVoiceManager.AudioInputDevices.Muted = true; 
        }
        else
        {
            if (lobbychannel.AudioState == ConnectionState.Disconnected)
            {
                // Ask for hosts since we're already in the channel and part added won't be triggered.

                lobbychannel.BeginSetAudioConnected(true, true, ar =>
                {
                    Debug.Log("Now transmitting into lobby channel");
                });
            }

        }
    }

    private void OnUserLoggedOut()
    {
        _vivoxVoiceManager.DisconnectAllChannels();

        LobbyScreen.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_chan == null || _chan.ChannelState != ConnectionState.Connected) return;
        var camera = Camera.main;
        if(camera == null) return;
        var cameraTransform = camera.transform;
        _chan.Set3DPosition(cameraTransform.position,cameraTransform.position,cameraTransform.forward, cameraTransform.up);
    }
    
    #endregion
}