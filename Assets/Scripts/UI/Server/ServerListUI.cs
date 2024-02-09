using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MultiplayApi.Common;
using MultiplayApi.Service;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Server
{
    public class ServerListUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _connectionStatusText;
        [SerializeField] private GameObject _serverPanelPrefab;
        [SerializeField] private Transform _serverPanelParent;

        private const string KeyId = "faa78a72-a769-4594-a20b-8c677901adb6";
        private const string SecretId = "DiAfnaW4MhJJSY1baH0sS6AnnJUX8KML";
        private const string ProjectId = "093ae33f-9b56-4e1a-a233-08ad3438b76c";
        private const string EnvironmentId = "5105ae74-6981-4eb6-89a4-9da20b640c13";
        private const string FleetId = "001918ba-7011-4fe5-abfb-cac116569c61";
        private const string EuropeRegionId = "0548345a-8510-49a8-80c8-ae8ce00fc934";
        private const int BuildConfigId = 1254014;

        private IMultiplayWebApi MultiplayWebApi = new MultiplayWebApi(KeyId, SecretId, ProjectId, EnvironmentId,
            FleetId, EuropeRegionId, BuildConfigId);

        private bool _authenticated;

        private void OnEnable()
        {
            if (!_authenticated) return;
            DisplayServerList();
        }
        
        private async void Start()
        {
            _connectionStatusText.text = "Waiting for connection...";
            await MultiplayWebApi.Authenticate();
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);
            DisplayServerList();
            _authenticated = true;
        }
        
        private async UniTask AllocateServer(ServerData server)
        {
            IMultiplayWebApi multiplayWebApi = new MultiplayWebApi(KeyId, SecretId, ProjectId, EnvironmentId,
                server.FleetID, EuropeRegionId, BuildConfigId);
            await multiplayWebApi.Authenticate();
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);
            var allocationId = await multiplayWebApi.AllocateServer();
            var serverById = await MultiplayWebApi.GetServerById(allocationId);

            while (string.IsNullOrWhiteSpace(serverById.Ipv4))
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                serverById = await multiplayWebApi.GetServerById(allocationId);
                GetComponent<ServerPanelUI>().SetAllocatedServer(serverById.Ipv4, serverById.GamePort);
            }
        }

        private async void DisplayServerList()
        {
            foreach (Transform child in _serverPanelParent)
                Destroy(child.gameObject);
            var serversList = await MultiplayWebApi.GetServersList();

            foreach (var serv in serversList)
            {
                var serverPanel = Instantiate(_serverPanelPrefab, _serverPanelParent);
                var serverPanelUI = serverPanel.GetComponent<ServerPanelUI>();
                serverPanelUI.SetServer(serv);

                if (serv.Status != ServerStatus.Allocated) 
                    AllocateServer(serv);
            }
            
            _connectionStatusText.gameObject.SetActive(false); // Hide the connection status text
        }
    }
}