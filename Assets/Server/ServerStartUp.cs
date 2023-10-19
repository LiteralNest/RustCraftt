using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using UnityEngine;

public class ServerStartUp : MonoBehaviour
{
    [SerializeField] private ushort _serverPort = 7777;
    [SerializeField] private ushort _maxPlayer = 10;
    
    private const string InternalServerIp = "0.0.0.0";
    private IMultiplayService _multiplayServices;
    private MultiplayEventCallbacks _serverCallBacks;
    private IServerEvents _serverEvents;

    async void Start()
    {
        bool isServer = false;
        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-dedicatedServer")
            {
                isServer = true;
                continue;
            }

            if (args[i] == "-port" && (i + 1 < args.Length))
                _serverPort = (ushort)int.Parse(args[i + 1]);
        }

        if (isServer)
        {
            StartServer();

            await StartServerServices();
        }
    }

    private void StartServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(InternalServerIp, _serverPort);
        NetworkManager.Singleton.StartServer();
    }
    
    async Task StartServerServices()
    {
        await UnityServices.InitializeAsync();

        try
        {
            _multiplayServices = MultiplayService.Instance;
            await _multiplayServices.StartServerQueryHandlerAsync(_maxPlayer, "n/a", "n/a", "0", "n/a");
        }

        catch (Exception ex)
        {
            Debug.LogError($"Something went wrong trying to set up the SQP:\n{ex}");
        }
    }
}