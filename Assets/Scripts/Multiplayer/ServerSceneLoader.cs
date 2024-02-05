#if UNITY_EDITOR
using ParrelSync;
#endif

using Server;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class ServerSceneLoader : MonoBehaviour
{
    [SerializeField] private bool _shouldLoadServer;
    [SerializeField] private bool _shouldLoadHost;
    [SerializeField] private bool _shouldLoadClient;
    [SerializeField] private ConnectionManager _connectionManager;

    private void Start()
    {
#if UNITY_EDITOR
        if (ClonesManager.IsClone())
        {
            if (_shouldLoadServer)
            {
                NetworkManager.Singleton.StartClient();
                return;
            }

            NetworkManager.Singleton.StartServer();
            return;
        }
#endif

#if !UNITY_SERVER

        if (_shouldLoadServer)
        {
            NetworkManager.Singleton.StartServer();
            return;
        }

        if (_shouldLoadHost)
        {
            NetworkManager.Singleton.StartHost();
            return;
        }

        if (_shouldLoadClient)
        {
            NetworkManager.Singleton.StartClient();
            return;
        }

        _connectionManager.Connect();
#endif
    }
}