using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cloud.DataBaseSystem.DataBaseServices.UserAuthorization
{
    public class UserAuthorization
    {
        [Button]
        public async Task<string> RegisterPlayerAsync(string login, string password)
        {
            WWWForm form = new();
            form.AddField("name", login);
            form.AddField("password", password);
            WWW www = new(DataBaseLinks.RegisterLink, form);
            await www;
            return www.text;
        }

        [Button]
        public async Task<string> LoginPlayerAsync(string login, string password)
        {
            WWWForm form = new();
            form.AddField("name", login);
            form.AddField("password", password);
            WWW www = new(DataBaseLinks.LoginLink, form);
            await www;
            return www.text;
        }
    }
}