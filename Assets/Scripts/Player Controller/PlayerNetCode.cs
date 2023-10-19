using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetCode : NetworkBehaviour
{
    public static PlayerNetCode Singleton { get; private set; }
    
    public NetworkVariable<int> ActiveItemId = new NetworkVariable<int>();

    private NetworkVariable<ulong> _gettedClientId = new NetworkVariable<ulong>();

    [SerializeField] private InHandObjectsContainer _inHandObjectsContainer;

    private void OnEnable()
        => GlobalEventsContainer.ShouldDisplayHandItem += SendChangeInHandItem;

    private void OnDisable()
        => GlobalEventsContainer.ShouldDisplayHandItem -= SendChangeInHandItem;

    private void Start()
    {
        if (IsOwner)
            Singleton = this;
    }
    
    public override void OnNetworkSpawn()
    {
        GlobalEventsContainer.OnNetworkPlayerSpawned?.Invoke();
        _gettedClientId.Value = GetClientId();

        ActiveItemId.OnValueChanged += (int prevValue, int newValue) =>
        {
            if(GetClientId() != _gettedClientId.Value) return;
            _inHandObjectsContainer.DisplayItems(ActiveItemId.Value);
        };

        // ViVoxInitialise();
    }

  
    private void ViVoxInitialise()
    {
        // var vTog = GameObject.Find("Toggle").GetComponent<Toggle>();
        // if (vTog.isOn)
        // {
        //     VivoxVoiceTesting vivoxVoiceTesting = FindObjectOfType<VivoxVoiceTesting>();
        //     if (IsClient)
        //     {
        //         if (vivoxVoiceTesting != null)
        //         {
        //             Debug.Log("2222");
        //             vivoxVoiceTesting.SignIntoVivox();
        //         }
        //     }
        // }
    }
    public bool PlayerIsOwner()
        => IsOwner;

    
    public ulong GetClientId()
        => OwnerClientId;

    private void SendChangeInHandItem(int itemId, ulong clientId)
    {
        if(!IsOwner) return;
        ChangeInHandItemServerRpc(itemId, clientId);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void ChangeInHandItemServerRpc(int itemId, ulong clientId)
    {
        // if(!IsOwner) return;
        ActiveItemId.Value = itemId;
        _gettedClientId.Value = clientId;
    }

    [ContextMenu("Test")]
    public void TestRPC()
    {
        ChangeInHandItemServerRpc(4, GetClientId());
    }
}