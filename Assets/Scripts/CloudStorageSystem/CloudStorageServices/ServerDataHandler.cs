using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using UnityEngine;

namespace CloudStorageSystem.CloudStorageServices
{
    public class ServerDataHandler
    {
        public IEnumerator SendDataCoroutine<T>(string key, T data) where T : struct
        {
            var sendingData = new Dictionary<string, object> { { key, data } };
            yield return CloudSaveService.Instance.Data.ForceSaveAsync(sendingData);
            Debug.Log("Data with key " + key + " has been sent");
        }

        public async Task<T> LoadDataAsync<T>(string key) where T : struct
        {
            var query = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });
            var data = query[key];
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}