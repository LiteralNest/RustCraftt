using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cloud.DataBaseSystem.DataBaseServices
{
    public class DataBaseUserGetter
    {
        [Button]
        public async Task<string> PlayerExistsAsync(string login)
        {
            WWWForm form = new();
            form.AddField("name", login);
            WWW www = new(DataBaseLinks.GetUserLink, form);
            await www;
            return www.text;
        }
    }
}