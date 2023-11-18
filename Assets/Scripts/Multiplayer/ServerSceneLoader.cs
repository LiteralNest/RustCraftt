using Server;
using Unity.Netcode;
using UnityEngine;

public class ServerSceneLoader : MonoBehaviour
{
    [SerializeField] private bool _shouldLoadHost;
    [SerializeField] private ConnectionManager _connectionManager;

    private void Start()
    {
#if !UNITY_SERVER
        if (_shouldLoadHost)
        {
            NetworkManager.Singleton.StartHost();
            return;
        }
        _connectionManager.Connect();
#endif
    }
}