using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MultiplayApi.Service;
using TMPro;
using Unity.Netcode.Samples;
using UnityEngine;

namespace UI
{
    public class ServerListUI : MonoBehaviour
    {
        [SerializeField] private GameObject _serverPanelPrefab;
        [SerializeField] private Transform _serverPanelParent;

        private const string KeyId = "faa78a72-a769-4594-a20b-8c677901adb6";
        private const string SecretId = "DiAfnaW4MhJJSY1baH0sS6AnnJUX8KML";
        private const string ProjectId = "093ae33f-9b56-4e1a-a233-08ad3438b76c";
        private const string EnvironmentId = "5105ae74-6981-4eb6-89a4-9da20b640c13";
        private const string FleetId = "001918ba-7011-4fe5-abfb-cac116569c61";
        private const string EuropeRegionId = "0548345a-8510-49a8-80c8-ae8ce00fc934";
        private const int BuildConfigId = 1253306;

        private IMultiplayWebApi MultiplayWebApi = new MultiplayWebApi(KeyId, SecretId, ProjectId, EnvironmentId,
            FleetId, EuropeRegionId, BuildConfigId);

        private async void Start()
        {
            await MultiplayWebApi.Authenticate();
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);
            DisplayServerList();
        }

        private async void DisplayServerList()
        {
            var serversList = await MultiplayWebApi.GetServersList();
            
            Unity.Netcode.Samples.ServerData server = null;
            foreach (var x in serversList)
            {
                if (x.Status == ServerStatus.Allocated)
                {
                    server = x;
                    break;
                }
            }
            foreach (var serv in serversList)
            {
                if (server == null) continue;
                var serverPanel = Instantiate(_serverPanelPrefab, _serverPanelParent);
                var serverPanelUI = serverPanel.GetComponent<ServerPanelUI>();
                serverPanelUI.SetServerDataUI(serv.IP, serv.LocationName);
                serverPanelUI.SetServer(serv.IP, serv.Port);
            }
            
            Allocate();
        }

        private async void Allocate()
        {
            var allocationId = await MultiplayWebApi.AllocateServer();
            var serverById = await MultiplayWebApi.GetServerById(allocationId);
         
            while (string.IsNullOrWhiteSpace(serverById.Ipv4))
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                
                serverById = await MultiplayWebApi.GetServerById(allocationId);
                GetComponent<ServerPanelUI>().SetAllocatedServer(serverById.Ipv4, serverById.GamePort);
            }
        }
        
        
    }
}