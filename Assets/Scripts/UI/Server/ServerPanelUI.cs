using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using MultiplayApi.Common;
using TMPro;
using UnityEngine;
using Ping = UnityEngine.Ping;

namespace UI.Server
{
    public class ServerPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _serverInfo;
        [SerializeField] private TMP_Text _pingText;

        private string _serverIp;
        private int _serverPort;
        private string _allocateServerIpv4;
        private int _allocateServerGamePort;

        public void SetServer(ServerData serverData)
        {
            _serverIp = serverData.IP;
            _serverPort = serverData.Port;
            _serverInfo.text = $"{serverData.IP} - {serverData.LocationName}";
            StartCoroutine(DisplayPingTextRoutine(serverData.IP));
        }

        private IEnumerator DisplayPingTextRoutine(string ip)
        {
            Ping ping = new Ping(ip);
            while (!ping.isDone) yield return null;
            _pingText.text = ping.time.ToString();
        }

        public void SetAllocatedServer(string allocateServerIpv4, int allocateServerGamePort)
        {
            _allocateServerIpv4 = allocateServerIpv4;
            _allocateServerGamePort = allocateServerGamePort;
        }

        public void OnServerSelected()
        {
            ServerSelectionData.SelectedServerIp = _serverIp;
            ServerSelectionData.SelectedServerPort = _serverPort;
            ServerSelectionData.AllocateServerIpv4 = _allocateServerIpv4;
            ServerSelectionData.AllocateServerGamePort = _allocateServerGamePort;

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}