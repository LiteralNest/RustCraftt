using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Cloud.DataBaseSystem.DataBaseServices.ServerData
{
    public class ServerDataBaseHandler
    {
        public async Task<string> UpdateServerDataAsync(string login, string playersCount)
        {
            WWWForm form = new();
            form.AddField("ip", login);
            form.AddField("playersCount", playersCount);
            WWW www = new(DataBaseLinks.UpdateServerDataLink, form);
            await www;
            return www.text;
        }
    }
}