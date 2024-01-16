using ParrelSync;
using Server;
using Unity.Netcode;
using UnityEngine;

public class ServerSceneLoader : MonoBehaviour
{
    [SerializeField] private bool _shouldLoadServer;
    [SerializeField] private bool _shouldLoadHost;
    [SerializeField] private ConnectionManager _connectionManager;

    private void Start()
    {
#if UNITY_EDITOR
        if (ClonesManager.IsClone())
        {
            NetworkManager.Singleton.StartClient();
            return;
        }
#endif

#if !UNITY_SERVER
        
        if(_shouldLoadServer)
        {
            NetworkManager.Singleton.StartServer();
            return;
        }
        
        if (_shouldLoadHost)
        {
            NetworkManager.Singleton.StartHost();
            return;
        }

        _connectionManager.Connect();
#endif
    }
}