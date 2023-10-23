using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using UnityEngine;

public class ClientStartUp : MonoBehaviour
{
    [SerializeField] private string _serverAddress = "127.0.0.1";//need to change
    [SerializeField] private ushort _serverPort = 7777;//need to change

    async void Start()
    {
        await InitializeClientAsync();
    }

    public async Task InitializeClientAsync()
    {
        await UnityServices.InitializeAsync();

        // could make authorization here

        StartClient();
    }

    private void StartClient()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_serverAddress, _serverPort);
        NetworkManager.Singleton.StartClient();
    }
}
