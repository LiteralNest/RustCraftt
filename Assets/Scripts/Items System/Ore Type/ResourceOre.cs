using UnityEngine;

public class ResourceOre : Ore
{
    [SerializeField] private int _hp;
    private int _currentHp;

    private void Start()
    {
        gameObject.tag = "Ore";
        _currentHp = _hp;
    }
    
    public async void MinusHp()
    {
        if(_currentHp <= 0) return;
        _currentHp--;
        InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(_targetResource, 2);
        if(_currentHp > 0) return;
        await Destroy();
        _currentHp = _hp;
    }
}