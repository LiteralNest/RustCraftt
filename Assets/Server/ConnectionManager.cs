using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MultiplayApi.Service;
using UI;
using Unity.Netcode;
using Unity.Netcode.Samples;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Server
{
    public class ConnectionManager : MonoBehaviour
    {
        private const string KeyId = "faa78a72-a769-4594-a20b-8c677901adb6";
        private const string SecretId = "DiAfnaW4MhJJSY1baH0sS6AnnJUX8KML";
        private const string ProjectId = "093ae33f-9b56-4e1a-a233-08ad3438b76c";
        private const string EnvironmentId = "5105ae74-6981-4eb6-89a4-9da20b640c13";
        
        private const string FleetId = "001918ba-7011-4fe5-abfb-cac116569c61";
        private const string EuropeRegionId = "0548345a-8510-49a8-80c8-ae8ce00fc934";
        private const int BuildConfigId = 1253306;

        private ServerDataUI _serverDataUI;

        
        public IMultiplayWebApi MultiplayWebApi { get; private set; } = new MultiplayWebApi(KeyId, SecretId, ProjectId, EnvironmentId,
            FleetId, EuropeRegionId, BuildConfigId);
     
        private async void Awake()
        {
            _serverDataUI = ServerDataUI.Instance;
            await MultiplayWebApi.Authenticate();
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);
            // Debug.LogError("Auth Completed");
        }

        public async void Connect()
        {
            var networkManager = NetworkManager.Singleton;
            var transport = networkManager.GetComponent<UnityTransport>();

            var serversList = await MultiplayWebApi.GetServersList();
            var server = serversList.FirstOrDefault(x => x.Status == ServerStatus.Allocated);

            if (server != null)
            {
                transport.SetConnectionData(server.IP, (ushort)server.Port);
                _serverDataUI.Region = server.LocationName;
                _serverDataUI.Ip = server.IP;
                networkManager.StartClient();
                return;
            }

            var allocationId = await MultiplayWebApi.AllocateServer();
            var serverById = await MultiplayWebApi.GetServerById(allocationId);
            while (string.IsNullOrWhiteSpace(serverById.Ipv4))
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                
                serverById = await MultiplayWebApi.GetServerById(allocationId);
            }
            transport.SetConnectionData(serverById.Ipv4, (ushort)serverById.GamePort);
            _serverDataUI.Region = serverById.RegionId;
            _serverDataUI.Ip = serverById.Ipv4;
            networkManager.StartClient();
        }

      
    }
}
