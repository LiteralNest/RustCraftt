using System.Collections.Generic;
using UnityEngine;

public class ResourceOre : Ore
{
    [SerializeField] private int _hp;
    private int _currentHp;

    [SerializeField] private List<Item> _toolsForGathering = new List<Item>();

    private void Start()
    {
        gameObject.tag = "Ore";
        _currentHp = _hp;
    }

    private bool CanUseTool(Item tool)
        => _toolsForGathering.Contains(tool);

    public async void MinusHp(Item targetTool)
    {
        if (_currentHp <= 0) return;
        if(!CanUseTool(targetTool)) return;
        _currentHp--;
        InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(_targetResource, 2);
        if (_currentHp > 0) return;
        await Destroy();
        _currentHp = _hp;
    }
}