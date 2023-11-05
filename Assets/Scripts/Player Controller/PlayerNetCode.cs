using System.Collections.Generic;
using ArmorSystem.Backend;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Web.User;

public class PlayerNetCode : NetworkBehaviour
{
    public static PlayerNetCode Singleton { get; private set; }
    
    private NetworkVariable<ulong> _gettedClientId = new NetworkVariable<ulong>();
    
    [Header("In Hand Items")]
    public NetworkVariable<int> ActiveItemId = new NetworkVariable<int>();
    [SerializeField] private InHandObjectsContainer _inHandObjectsContainer;

    [Header("Armor")]
    public NetworkVariable<int> ActiveArmorId = new(101, NetworkVariableReadPermission.Everyone,
    NetworkVariableWritePermission.Owner);
    [SerializeField] private ArmorsContainer _armorsContainer;

    [Header("NickName")] [SerializeField] private NetworkVariable<int> _playerId = new(-1, NetworkVariableReadPermission.Everyone,
    NetworkVariableWritePermission.Owner);
    [SerializeField] private List<TMP_Text> _nickNameTexts = new List<TMP_Text>();

    private void OnEnable()
        => GlobalEventsContainer.ShouldDisplayHandItem += SendChangeInHandItem;

    private void OnDisable()
        => GlobalEventsContainer.ShouldDisplayHandItem -= SendChangeInHandItem;

    private void Start()
    {
#if UNITY_SERVER
        WebServerDataHandler.singleton.RegistrateNewUser();
#endif
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Singleton = this;
            _playerId.Value = UserDataHandler.singleton.UserData.Id;
        }
        _gettedClientId.Value = GetClientId();
        AssignName();
        
        ActiveItemId.OnValueChanged += (int prevValue, int newValue) =>
        {
            if (GetClientId() != _gettedClientId.Value) return;
            _inHandObjectsContainer.DisplayItems(ActiveItemId.Value);
        };

        ActiveArmorId.OnValueChanged += (int prevValue, int newValue) =>
        {
            if (GetClientId() != _gettedClientId.Value) return;
            _armorsContainer.DisplayArmor(ActiveArmorId.Value);
        };
        
        _playerId.OnValueChanged += (int prevValue, int newValue) =>
        {
            AssignName();
        };
        
        _inHandObjectsContainer.DisplayItems(ActiveItemId.Value);
        _armorsContainer.DisplayArmor(ActiveArmorId.Value);
    }
    
    private async void AssignName()
    {
        string name = await WebUserDataHandler.singleton.GetUserValueById(_playerId.Value);
        foreach (var nickNameText in _nickNameTexts)
            nickNameText.text = name;
    }

    public bool PlayerIsOwner()
        => IsOwner;


    public ulong GetClientId()
        => OwnerClientId;

    private void SendChangeInHandItem(int itemId, ulong clientId)
    {
        if (!IsOwner) return;
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
        ChangeInHandItemServerRpc(18, GetClientId());
    }
}