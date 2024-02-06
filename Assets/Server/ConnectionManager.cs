using UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Server
{
    public class ConnectionManager : MonoBehaviour
    {
        public void Connect()
        {
            var networkManager = NetworkManager.Singleton;
            var transport = networkManager.GetComponent<UnityTransport>();
            
            if (string.IsNullOrEmpty(ServerSelectionData.SelectedServerIp) || ServerSelectionData.SelectedServerPort == 0)
            {
                transport.SetConnectionData(ServerSelectionData.AllocateServerIpv4, (ushort)ServerSelectionData.AllocateServerGamePort);
            }
            else
            {
                transport.SetConnectionData(ServerSelectionData.SelectedServerIp, (ushort)ServerSelectionData.SelectedServerPort);
            }
            
            networkManager.StartClient();
        }

      
    }
}
