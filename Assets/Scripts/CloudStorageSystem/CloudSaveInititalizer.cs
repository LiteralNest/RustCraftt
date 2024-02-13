using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;

namespace CloudStorageSystem
{
    public class CloudSaveInititalizer : MonoBehaviour
    {
        private async void Start()
        {
            await TestSending();
            await LoadData();
        }

        private async Task TestSending()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            var data = new SavingData();
            data.Id = 1;
            data.Name = "Test";
            var sendingData = new Dictionary<string, object> { { "MySaveKey", data } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(sendingData);
        }

        private async Task LoadData()
        {
            var query = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "MySaveKey" });
            var data = query["MySaveKey"];
            var loadedData = JsonConvert.DeserializeObject<SavingData>(data);
            Debug.Log(loadedData.Name);
        }
    }
}