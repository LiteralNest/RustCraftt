using System.Collections;
using MultiplayApi.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ping = UnityEngine.Ping;

namespace UI.Server
{
    public class ServerButtonView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _serverInfo;
        [SerializeField] private TMP_Text _pingText;
        [SerializeField] private Button _loadButton; 

        private string _serverIp;
        private int _serverPort;
        private string _allocateServerIpv4;
        private int _allocateServerGamePort;

        private void Start()
        {
            _loadButton.onClick.RemoveAllListeners();
            _loadButton.onClick.AddListener(OnServerSelected);
            _loadButton.onClick.AddListener(() =>
            {
                LevelsLoader.singleton.LoadLevelAsync(1);
            });
        }

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

        private void OnServerSelected()
        {
            ServerSelectionData.SelectedServerIp = _serverIp;
            ServerSelectionData.SelectedServerPort = _serverPort;
            ServerSelectionData.AllocateServerIpv4 = _allocateServerIpv4;
            ServerSelectionData.AllocateServerGamePort = _allocateServerGamePort;

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}