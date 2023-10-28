using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MultiplayApi.Service;
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
        
        private const string FleetId = "f17bde87-612b-40d7-b479-8287da85d9bc";
        private const string EuropeRegionId = "0548345a-8510-49a8-80c8-ae8ce00fc934";
        private const int BuildConfigId = 1245491;

        public IMultiplayWebApi MultiplayWebApi { get; private set; } = new MultiplayWebApi(KeyId, SecretId, ProjectId, EnvironmentId,
            FleetId, EuropeRegionId, BuildConfigId);

        private async void Awake()
        {
            await MultiplayWebApi.Authenticate();
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);
            // Debug.LogError("Auth Completed");
        }

        private void OnGUI()
        {
            var customButtonStyle = new GUIStyle(GUI.skin.button);
            customButtonStyle.fontSize = 40;
            
            GUILayout.BeginArea(new Rect(50, 50, 400, 100));

            var networkManager = NetworkManager.Singleton;
            if (!networkManager.IsClient && !networkManager.IsServer)
            {
                // if (GUILayout.Button("Host")) networkManager.StartHost();
                GUILayout.Space(20);
                if (GUILayout.Button("Client",customButtonStyle, GUILayout.Width(400), GUILayout.Height(100))) Connect();


                // if (GUILayout.Button("Server")) networkManager.StartServer();
            }
            else
                GUILayout.Label($"Mode: {(networkManager.IsHost ? "Host" : networkManager.IsServer ? "Server" : "Client")}");

            GUILayout.EndArea();
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
            networkManager.StartClient();
        }
    }
}
