using System;
#if UNITY_SERVER
using Unity.Services.Multiplay;
#endif
using UnityEngine;

public class MultiplayerCallbacks : MonoBehaviour
{
#if UNITY_SERVER
    public event Action<MultiplayAllocation> Allocate;
    public event Action<MultiplayDeallocation> Dealocate;
    
    private MultiplayEventCallbacks _multiplayEventCallbacks;
    private IServerEvents _serverEvents;

    private async void Start()
    {
        _multiplayEventCallbacks = new MultiplayEventCallbacks();
        _multiplayEventCallbacks.Allocate += OnAllocate;
        _multiplayEventCallbacks.Deallocate += OnDeallocate;
        _multiplayEventCallbacks.Error += OnError;
        _multiplayEventCallbacks.SubscriptionStateChanged += OnSubscriptionStateChanged;

        _serverEvents = await MultiplayService.Instance.SubscribeToServerEventsAsync(_multiplayEventCallbacks);
    }

    private void OnSubscriptionStateChanged(MultiplayServerSubscriptionState state)
    {
        switch (state)
        {
            case MultiplayServerSubscriptionState.Unsubscribed :
                Debug.Log("Unsubscribed!");
                break;
            case MultiplayServerSubscriptionState.Synced :
                Debug.Log("Synced!");
                break;
            case MultiplayServerSubscriptionState.Unsynced :
                Debug.Log("Unsynced!");
                break;
            case MultiplayServerSubscriptionState.Error :
                Debug.Log("Error!"); 
                break;
            case MultiplayServerSubscriptionState.Subscribing :
                Debug.Log("Subscribing!");
                break;
        }
    }

    private void OnError(MultiplayError error)
    {
        Debug.Log(error.ToString());
    }

    private void OnDeallocate(MultiplayDeallocation deallocation)
    {
        Debug.Log("Server is deallocated!");
        
        Dealocate?.Invoke(deallocation);
    }


    private void OnAllocate(MultiplayAllocation allocation)
    {
        Debug.Log("Server is deallocated!");
        LogServerConfig();
        Allocate?.Invoke(allocation);
    }
    
    private void LogServerConfig()
    {
        var serverConfig = MultiplayService.Instance.ServerConfig;
        Debug.Log($"Server ID[{serverConfig.ServerId}]");
        Debug.Log($"AllocationID[{serverConfig.AllocationId}]");
        Debug.Log($"Port[{serverConfig.Port}]");
        Debug.Log($"QueryPort[{serverConfig.QueryPort}");
        Debug.Log($"LogDirectory[{serverConfig.ServerLogDirectory}]");
    }
#endif
}