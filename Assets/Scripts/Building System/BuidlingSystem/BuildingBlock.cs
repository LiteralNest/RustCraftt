using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : DamagableBuilding
{
    [field:SerializeField] public List<InventoryCell> NeededCellsForPlace = new List<InventoryCell>();
    [SerializeField] private List<GameObject> _levels = new List<GameObject>();
    
    private int _currentLevel;
    private GameObject _activeBlock;
    
    private void Start()
    {
        InitSlot(0);
    }

    private void InitSlot(int slotId)
    {
        _currentLevel = slotId;
        if (_activeBlock != null)
            _activeBlock.SetActive(false);
        var activatingBlock = _levels[_currentLevel].gameObject;
        activatingBlock.SetActive(true);
        _activeBlock = activatingBlock;
    }

    [ContextMenu("Upgrade")]
    public void Upgrade()
        => InitSlot(_currentLevel + 1);
    
    
    public void Repair()
    {
        //Додати Needed Resources for upgrade
    }
}