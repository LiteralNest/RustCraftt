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
    }

    private bool CanUseTool(Item tool)
        => _toolsForGathering.Contains(tool);
    
    public async void MinusHp(Item targetTool)
    {
        if (_currentHp.Value <= 0) return;
        if(!CanUseTool(targetTool)) return;
        _currentHp.Value--;
        InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(_targetResource, 2);
        if (_currentHp.Value > 0) return;
        await Destroy();
        _currentHp.Value = _hp;
    }

    [ContextMenu("Test Destroy")]
    private void TestDestroy()
    {
        Destroy();
    }
}