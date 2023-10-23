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

    private const string InternalServerIp = "0.0.0.0";

    async void Start()
    {
        await InitializeServerAsync();
    }

    public async Task InitializeServerAsync()
    {
        await UnityServices.InitializeAsync();

        try
        {
            IMultiplayService multiplayServices = MultiplayService.Instance;
            await multiplayServices.StartServerQueryHandlerAsync(4, "n/a", "n/a", "n/a", "n/a");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Something went wrong trying to set up the SQP:\n{ex}");
        }

        StartServer();
    }

    private void StartServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(InternalServerIp, _serverPort);
        NetworkManager.Singleton.StartServer();
    }
}

