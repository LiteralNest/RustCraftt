using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using UnityEngine;

public class InitMultiplay : MonoBehaviour
{
#if UNITY_SERVER
    private const ushort DefaultMaxPlayers = 10;
    private const string DefaultServerName = "MyServer";
    private const string DefaultGameType = "MyGameType";
    private const string DefaultBuildId = "MyBuildId";
    private const string DefaultMap = "MyMap";

    private ushort _currentPlayers;
    private IServerQueryHandler _serverQueryHandler;
    private bool _isInitialized;

    private bool _waiting;

    private async void Awake()
    {
        await InitSDK();
        LogServerConfig();
        await ReadyingServer();

        _serverQueryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync(DefaultMaxPlayers,
            DefaultServerName, DefaultGameType, DefaultBuildId, DefaultMap);

        var config = MultiplayService.Instance.ServerConfig;
        var transport = GetComponent<UnityTransport>();

        const string ipv4Address = "0.0.0.0";
        transport.SetConnectionData(ipv4Address, config.Port, ipv4Address);

        var networkManager = GetComponent<NetworkManager>();
        networkManager.StartServer();
        
        Debug.Log("Init Server...");
    }

    #region SDK

    private static async Task InitSDK()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static void LogServerConfig()
    {
        var serverConfig = MultiplayService.Instance.ServerConfig;
        Debug.Log($"Server IP[{serverConfig.IpAddress}]");
        Debug.Log($"Server ID[{serverConfig.ServerId}]");
        Debug.Log($"AllocationID[{serverConfig.AllocationId}]");
        Debug.Log($"Port[{serverConfig.Port}]");
        Debug.Log($"QueryPort[{serverConfig.QueryPort}");
        Debug.Log($"LogDirectory[{serverConfig.ServerLogDirectory}]");
    }

    private static async Task ReadyingServer()
    {
        // After the server is back to a blank slate and ready to accept new players
        await MultiplayService.Instance.ReadyServerForPlayersAsync();
    }

    #endregion
    

    private async void UpdateData()
    {
        _waiting = true;
        await Task.Delay(100);
        if (_serverQueryHandler == null)
            Debug.Log("ServerQueryHandler is null");
        else
            _serverQueryHandler.UpdateServerCheck();
        _waiting = false;
    }

    private void Update()
    {
        // if (!_isInitialized) return;
        if (_waiting) return;
        UpdateData();
    }
#endif
}