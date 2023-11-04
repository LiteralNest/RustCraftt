using UnityEngine;
using UnityEngine.Android;
using Web.User;

public class VivoxStartLoginer : MonoBehaviour
{
    private int PermissionAskedCount = 0;
    
    private void Awake()
        => TryLoginToVivox();

    private void TryLoginToVivox()
    {
        var data = UserDataHandler.singleton; 
        if (data == null) return;
        LoginToVivoxService(data);
    }
    
    private bool IsMicPermissionGranted()
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
    
    private void LoginToVivoxService(UserDataHandler data)
    {
        if (IsMicPermissionGranted())
        {
            VivoxVoiceManager.singleton.Login(data.UserData.Name);
        }
        else
        {
            // We do not have the needed permissions.
            // Ask for permissions or proceed without the functionality enabled if they were denied by the user
            if (IsPermissionsDenied())
            {
                PermissionAskedCount = 0;
                VivoxVoiceManager.singleton.Login(data.UserData.Name);
            }
            else
            {
                AskForPermissions();
            }
        }
    }
}