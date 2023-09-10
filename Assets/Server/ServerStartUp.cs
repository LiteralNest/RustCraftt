using UnityEngine;

public class ServerStartUp : MonoBehaviour
{
    [SerializeField] private int _serverPort = 7777;
    private string _internalServerIp = "0.0.0.0";
    private void Start()
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
        
        if(!isServer) return;
        StartServer();
    }

    private void StartServer()
    {
        // NetworkManager.singleton.StartServer();
    }
}