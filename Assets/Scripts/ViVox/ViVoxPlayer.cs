using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.Android;
using VivoxUnity;

public class VivoxPlayer : MonoBehaviour
{
    private VivoxVoiceManager _vvm;
    IChannelSession _chan;
    private int PermissionAskedCount;
    [SerializeField] public string VoiceChannelName = "BloodRustChannel";

    private Transform _camera; //position of our Main Camera

    // Start is called before the first frame update
    private void Awake()
    {
        _vvm = VivoxVoiceManager.Instance;
        _vvm.OnUserLoggedInEvent += OnUserLoggedIn;
        _vvm.OnUserLoggedOutEvent += OnUserLoggedOut;

        //Need to discuss how to this in better way
        // xrCam = GameObject.Find("Main Camera").transform;
        if (NetworkManager.Singleton.IsHost)
        {
            _camera = transform.Find("Main Camera");
            _camera.GetComponent<AudioListener>().enabled = true;
        }
        else
        {
            _camera = transform.Find("Secondary Camera");
            _camera.GetComponent<AudioListener>().enabled = true;
        }
    }

    public void SignIntoVivox ()
    {
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
    private bool IsAndroid12AndUp()
    {
        // android12VersionCode is hardcoded because it might not be available in all versions of Android SDK
        const int android12VersionCode = 31;
        AndroidJavaClass buildVersionClass = new AndroidJavaClass("android.os.Build$VERSION");
        int buildSdkVersion = buildVersionClass.GetStatic<int>("SDK_INT");

        return buildSdkVersion >= android12VersionCode;
    }

    private string GetBluetoothConnectPermissionCode()
    {
        if (IsAndroid12AndUp())
        {
            // UnityEngine.Android.Permission does not contain the BLUETOOTH_CONNECT permission, fetch it from Android
            AndroidJavaClass manifestPermissionClass = new AndroidJavaClass("android.Manifest$permission");
            string permissionCode = manifestPermissionClass.GetStatic<string>("BLUETOOTH_CONNECT");

            return permissionCode;
        }
        return "";
    }
#endif

        bool IsMicPermissionGranted()
        {
            bool isGranted = Permission.HasUserAuthorizedPermission(Permission.Microphone);
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (IsAndroid12AndUp())
        {
            // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission for all features to work
            isGranted &= Permission.HasUserAuthorizedPermission(GetBluetoothConnectPermissionCode());
        }
#endif
            return isGranted;
        }

        void AskForPermissions()
        {
            var permissionCode = Permission.Microphone;

#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (PermissionAskedCount == 1 && IsAndroid12AndUp())
        {
            permissionCode = GetBluetoothConnectPermissionCode();
        }
#endif
            PermissionAskedCount++;
            Permission.RequestUserPermission(permissionCode);
        }

        bool IsPermissionsDenied()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission
        if (IsAndroid12AndUp())
        {
            return PermissionAskedCount == 2;
        }
#endif
            return PermissionAskedCount == 1;
        }

        //Actual code runs from here
        if (IsMicPermissionGranted())
        {
            _vvm.Login(transform.name);
        }
        else
        {
            if (IsPermissionsDenied())
            {
                PermissionAskedCount = 0;
                _vvm.Login(transform.name);
            }
            else
            {
                AskForPermissions();
            }
        }
    }

    private void OnUserLoggedIn ()
    {
        if (_vvm.LoginState == LoginState.LoggedIn)
        {
            Debug.Log("Successfully connected to Vivox");
            Debug.Log("Joining voice channel: " + VoiceChannelName);
            //_vvm.JoinChannel(VoiceChannelName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.AudioOnly);
            _vvm.JoinChannel(VoiceChannelName, ChannelType.Positional, VivoxVoiceManager.ChatCapability.AudioOnly);

            var cid = new Channel(VoiceChannelName, ChannelType.Positional);
            _chan = _vvm.LoginSession.GetChannelSession(cid);
        }
        else
        {
            Debug.Log("Cannot sign into Vivox, check your credentials and token settings");
        }
    }

    void OnUserLoggedOut()
    {
        Debug.Log("Disconnecting from voice channel " + VoiceChannelName);
        _vvm.DisconnectAllChannels();
        Debug.Log("Disconnecting from Vivox");
        _vvm.Logout();  
    }

    // Update is called once per frame
    private void Update()
    {
        float nextUpdate = 0;

        if (_chan == null)
            return;
        
        if (_chan.ChannelState.ToString() == "Connected")
        {
            if (Time.time > nextUpdate)
            {
                _chan.Set3DPosition(_camera.position, _camera.position, _camera.forward, _camera.up);
                nextUpdate += 0.5f;//delay of speech
            }
        }
        
    }
}