using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Cloud.CloudStorageSystem.CloudStorageServices
{
    public class CloudSaveInititalizer : MonoBehaviour
    {
        private async void Awake()
        {
            await UnityServices.InitializeAsync();
            if(AuthenticationService.Instance.IsAuthorized == false)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            CloudSaveEventsContainer.OnCloudSaveServiceInitialized?.Invoke();
        }
    }
}