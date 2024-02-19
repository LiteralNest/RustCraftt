using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Cloud.DataBaseSystem.DataBaseServices.ServerData
{
    public class ServerDataBaseHandler
    {
        public async Task<string> UpdateServerDataAsync(string serverIp, string playersCount)
        {
            WWWForm form = new();
            form.AddField("ip", serverIp);
            form.AddField("playersCount", playersCount);
            WWW www = new(DataBaseLinks.UpdateServerDataLink, form);
            await www;
            return www.text;
        }

        public async Task<string> GetServerDataAsync(string serverIp)
        {
            WWWForm form = new();
            form.AddField("ip", serverIp);
            WWW www = new(DataBaseLinks.GetServerDataLink, form);
            await www;
            return www.text;
        }
    }
}