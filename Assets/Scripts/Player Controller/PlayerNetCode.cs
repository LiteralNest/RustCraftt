using Unity.Netcode;
using UnityEngine;

public class PlayerNetCode : NetworkBehaviour
{
    public static PlayerNetCode singleton { get; private set; }
    
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
            singleton = this;
    }
    
    public override void OnNetworkSpawn()
    {
        _gettedClientId.Value = GetClientId();

        ActiveItemId.OnValueChanged += (int prevValue, int newValue) =>
        {
            Debug.Log("Checking id: getted client id:" + _gettedClientId.Value + " | Current Client id:" + GetClientId());
            if(GetClientId() != _gettedClientId.Value) return;
            Debug.Log("Id check!");
            _inHandObjectsContainer.DisplayItems(ActiveItemId.Value);
        };
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
        Debug.Log("Setted");
    }

    [ContextMenu("Test")]
    public void TestRPC()
    {
        ChangeInHandItemServerRpc(4, GetClientId());
    }
}