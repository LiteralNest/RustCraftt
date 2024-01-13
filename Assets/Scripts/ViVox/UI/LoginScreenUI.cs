using System.Collections;
using Console;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using Web.UserData;

namespace ViVox.UI
{
    public class LoginScreenUI : MonoBehaviour
    {
        private VivoxVoiceManager _vivoxVoiceManager;

        private int PermissionAskedCount = 0;

        #region Unity Callbacks

        private EventSystem _evtSystem;


        private void Awake()
        {
            _evtSystem = FindObjectOfType<EventSystem>();
            _vivoxVoiceManager = VivoxVoiceManager.Instance;
        }

        private void Start()
            => StartCoroutine(WaitForLogin());

        #endregion

        private IEnumerator WaitForLogin()
        {
            ConsoleDisplayer.Singleton?.DisplayText("Waiting for logging to Vivox sevice...");
            yield return new WaitForSeconds(2);
            LoginToVivoxService();
        }

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

        private bool IsMicPermissionGranted()
        {
            bool isGranted = Permission.HasUserAuthorizedPermission(Permission.Microphone);
            ConsoleDisplayer.Singleton?.DisplayText("Microphone permission granted: " + isGranted);
            return isGranted;
        }

        private void AskForPermissions()
        {
            string permissionCode = Permission.Microphone;

#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (PermissionAskedCount == 1 && IsAndroid12AndUp())
        {
            permissionCode = GetBluetoothConnectPermissionCode();
        }

#endif
            PermissionAskedCount++;
            Permission.RequestUserPermission(permissionCode);
        }

        private bool IsPermissionsDenied()
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

        [ContextMenu("LoginToVivoxService")]
        public void LoginToVivoxService()
        {
            if (IsMicPermissionGranted())
            {
                // The user authorized use of the microphone.
                LoginToVivox();
            }
            else
            {
                // We do not have the needed permissions.
                // Ask for permissions or proceed without the functionality enabled if they were denied by the user
                if (IsPermissionsDenied())
                {
                    PermissionAskedCount = 0;
                    LoginToVivox();
                }
                else
                {
                    AskForPermissions();
                }
            }
        }

        private void LoginToVivox()
        {
            if (string.IsNullOrEmpty(UserDataHandler.Singleton.UserData.Name))
            {
                var msg = UserDataHandler.Singleton.UserData.Name;
                ConsoleDisplayer.Singleton?.DisplayText(msg);
                Debug.LogError(msg);
                return; 
            }

            _vivoxVoiceManager.Login(UserDataHandler.Singleton.UserData.Name);
        }
    }
}