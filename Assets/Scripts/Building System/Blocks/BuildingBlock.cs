using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuildingBlock : NetworkBehaviour, IDamagable
{
    [SerializeField] private List<Block> _levels = new List<Block>();
    [field:SerializeField] public int Hp { get; private set; }
    [field:SerializeField] public BuildingConnector BuildingConnector { get; private set; }
    public Block CurrentBlock => _levels[_currentLevel];
    public List<InventoryCell> NeededCellsForPlace => _levels[_currentLevel].NeededCellsForPlace;

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
        var activatingBlock = _levels[_currentLevel];
        activatingBlock.gameObject.SetActive(true);
        _activeBlock = activatingBlock.gameObject;
        Hp = activatingBlock.GetComponent<Block>().Hp;
    }

    [ContextMenu("Upgrade")]
    public void Upgrade()
        => InitSlot(_currentLevel + 1);
    
    public void Repair()
    {
        //Додати Needed Resources for upgrade
    }

    public bool CanBeUpgraded()
        => _currentLevel < _levels.Count - 1;
    
    public void GetDamage(int damage)
    {
        Hp -= damage;
        if(Hp <= 0)
            Destroy(gameObject);
    }

    public Block GetUpgradingBlock()
    {
        if (_currentLevel == _levels.Count - 1)
            return _levels[_currentLevel];
        return _levels[_currentLevel + 1];
    }
}