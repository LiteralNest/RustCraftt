using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using UnityEngine;

public class ServerStartUp : MonoBehaviour
{
    [SerializeField] private NetworkManager _networkManager;

    //Could be made serializable
    private ushort _maxPlayer;
    private string _serverName = "BloodRust";
    private string _gameType = "Multiplayer";
    private string _buildID = Application.version;
    private string _map = "Default";

    private ushort _serverPort;


    private const string InternalServerIp = "0.0.0.0";
    private string _externalServerIp = "0.0.0.0";

    private IMultiplayService _multiplayService;
    private IServerQueryHandler _mServerQueryHandler;
    


    private async void Start()
    {
        var server = false;
       

        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-dedicatedServer")
            {
                server = true;
            }

            if (args[i] == "-port" && (i + 1 < args.Length))
            {
                _serverPort = (ushort)int.Parse(args[i + 1]);
            }   
            
            if (args[i] == "-ip" && (i + 1 < args.Length))
            {
                _externalServerIp = args[i + 1];
            }
        }

        if (server)
        {
            StartServer();
            await StartServerService();
        }
    }

    private void StartServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(InternalServerIp,_serverPort);
        NetworkManager.Singleton.StartServer();
    }

    private async Task StartServerService()
    {
        await UnityServices.InitializeAsync();
        try
        {
            _multiplayService = MultiplayService.Instance;
            
        }
        catch (Exception ex)
        {
            
        }
    }

    

}