using TMPro;
using UnityEngine;

namespace UI
{
    public class ServerPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _serverInfo;
        
        private string _serverIp;
        private int _serverPort;
        private string _allocateServerIpv4;
        private int _allocateServerGamePort;

        public void SetServer(string serverIp, int serverPort)
        {
            _serverIp = serverIp;
            _serverPort = serverPort;
        }

        public void SetAllocatedServer(string allocateServerIpv4, int allocateServerGamePort)
        {
            _allocateServerIpv4 = allocateServerIpv4;
            _allocateServerGamePort = allocateServerGamePort;
        }

        public void SetServerDataUI(string serverIp, string serverRegion)
        {
            _serverInfo.text = $"{serverIp} - {serverRegion}";
        }

        public void OnServerSelected()
        {
            ServerSelectionData.SelectedServerIp = _serverIp;
            ServerSelectionData.SelectedServerPort = _serverPort;
            ServerSelectionData.AllocateServerIpv4 = _allocateServerIpv4;
            ServerSelectionData.AllocateServerGamePort = _allocateServerGamePort;
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("");
        }
    }
}