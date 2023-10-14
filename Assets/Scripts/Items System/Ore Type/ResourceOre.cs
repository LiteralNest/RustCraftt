using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ResourceOre : Ore
{
    [SerializeField] private ushort _hp = 100;
    [SerializeField] private NetworkVariable<ushort> _currentHp = new(100, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    [SerializeField] private List<Item> _toolsForGathering = new List<Item>();

    private void Start()
    {
        gameObject.tag = "Ore";
        _currentHp.Value = _hp;
        _currentHp.OnValueChanged += (ushort prevValue, ushort newValue) =>
        {
            CheckHp();
        };
    }

    private bool CanUseTool(Item tool)
        => _toolsForGathering.Contains(tool);
    
    [ServerRpc(RequireOwnership = false)]
    public void MinusHpServerRpc()
    {
        _currentHp.Value--;
    }

    private async void CheckHp()
    {
        if (_currentHp.Value > 0) return;
        await Destroy();
        if(NetworkManager.Singleton.IsServer)
            _currentHp.Value = _hp;
    }

    public void MinusHp(Item targetTool, out bool destroyed)
    {
        destroyed = false;
        if (_currentHp.Value <= 0) return;
        if(!CanUseTool(targetTool)) return;
        InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(_targetResource, 1);
        MinusHpServerRpc();
        destroyed = _currentHp.Value <= 0;
    }

    [ContextMenu("Test RPC")]
    private void TestRpc()
    {
        MinusHpServerRpc();
    }
}