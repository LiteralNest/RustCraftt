using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ResourceOre : Ore
{
    [SerializeField] private List<Item> _toolsForGathering = new List<Item>();
    
    [field: SerializeField] public AudioClip GatheringClip { get; private set; }
    
    private void Start()
    {
        base.Start(); 
        gameObject.tag = "Ore";
    }
    private bool CanUseTool(Item tool)
        => _toolsForGathering.Contains(tool);

    public void MinusHp(Item targetTool, out bool destroyed)
    {
        destroyed = false;
        if (_currentHp.Value <= 0) return;
        if(!CanUseTool(targetTool)) return;
        InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(_targetResource, Random.Range(_addingCount.x, _addingCount.y + 1));
        MinusHpServerRpc();
        destroyed = _currentHp.Value <= 0;
    }

    [ContextMenu("Test RPC")]
    private void TestRpc()
    {
        MinusHpServerRpc();
    }
}