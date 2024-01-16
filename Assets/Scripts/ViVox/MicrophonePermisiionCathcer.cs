using UnityEngine;

namespace ViVox
{
    public class MicrophonePermisiionCathcer : MonoBehaviour
    {
        [SerializeField] private GameObject _microphoneGettedDisplay;

#if UNITY_ANDROID && !UNITY_EDITOR
        private void Start()
            => GetMicPermission();

        private bool CheckPermission(string permission)
        {
            new AndroidJavaClass("android.content.Context").GetRawClass();
            AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                .GetStatic<AndroidJavaObject>("currentActivity")
                .Call<AndroidJavaObject>("getApplicationContext");
            int permissionResult = context.Call<int>("checkSelfPermission", permission);
            return (permissionResult == 0);
        }

        private void GetMicPermission()
        {
            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            string permission = "android.permission.RECORD_AUDIO";

            if (!CheckPermission(permission))
            {
                currentActivity.Call("requestPermissions", new string[] { permission }, 1);
                //_microphoneGettedDisplay.SetActive(true);
            }

            if (CheckPermission(permission))
                _microphoneGettedDisplay.SetActive(true);
        }
#endif
    }
}