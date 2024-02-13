using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace CloudStorageSystem.CloudStorageServices
{
    public class CloudSaveInititalizer : MonoBehaviour
    {
        private async void Start()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            CloudSaveEventsContainer.OnCloudSaveServiceInitialized?.Invoke();
        }
    }
}