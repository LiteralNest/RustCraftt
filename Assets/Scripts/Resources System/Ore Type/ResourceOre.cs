using UnityEngine;

public class ResourceOre : Ore
{
    [SerializeField] private int _hp;
    private int _currentHp;

    private void Start()
    {
        _currentHp = _hp;
    }
    
    public async void MinusHp(InventorySlotsContainer inventory)
    {
        if(_currentHp <= 0) return;
        _currentHp--;
        inventory.AddItemToDesiredSlot(_targetResource, 2);
        if(_currentHp > 0) return;
        await Destroy();
        _currentHp = _hp;
    }
}