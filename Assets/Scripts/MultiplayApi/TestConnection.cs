using System;
using Unity.Netcode;
using UnityEngine;

public class TestConnection : NetworkBehaviour
{
    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnServerShutdown;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnServerShutdown;
    }

    public void OnConnectedToServer()
    {
        Debug.Log("Connected to server");
    }

    public void OnServerInitialized()
    {
        Debug.Log("Server initialized");
    }

    private void OnServerShutdown(ulong serverId)
    {
        Debug.Log("Server shutDown " + serverId);
    }
}
