using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ResourceOre : Ore
{
    [SerializeField] private List<Item> _toolsForGathering = new List<Item>();
    
    [field: SerializeField] public AudioClip GatheringClip { get; private set; }
    [SerializeField] private GameObject _vfxEffect;
    
    private void Start()
    {
        base.Start(); 
        gameObject.tag = "Ore";
    }
    private bool CanUseTool(Item tool)
        => _toolsForGathering.Contains(tool);

    public void MinusHp(Item targetTool, out bool destroyed, Vector3 lastRayPos, Vector3 lastRayRot)
    {
        destroyed = false;
        if (_currentHp.Value <= 0) return;
        if(!CanUseTool(targetTool)) return;
        InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(_targetResource, Random.Range(_addingCount.x, _addingCount.y + 1));
        Instantiate(_vfxEffect, lastRayPos,  Quaternion.FromToRotation(Vector3.up, lastRayRot));
        MinusHpServerRpc();
        destroyed = _currentHp.Value <= 0;
    }

    [ContextMenu("Test RPC")]
    private void TestRpc()
    {
        MinusHpServerRpc();
    }
}