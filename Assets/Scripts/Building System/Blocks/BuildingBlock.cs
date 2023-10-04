using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuildingBlock : NetworkBehaviour, IDamagable
{
    [SerializeField] private List<Block> _levels = new List<Block>();

    public NetworkVariable<ushort> Hp = new(100, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [field: SerializeField] public BuildingConnector BuildingConnector { get; private set; }
    public Block CurrentBlock => _levels[_currentLevel.Value];
    public List<InventoryCell> NeededCellsForPlace => _levels[_currentLevel.Value].NeededCellsForPlace;

    private ushort _startHp;
    private NetworkVariable<ushort> _currentLevel = new(0, NetworkVariableReadPermission.Everyone,
    NetworkVariableWritePermission.Owner);
    private GameObject _activeBlock;

    private void Start()
    {
        InitSlot(_currentLevel.Value);
        _currentLevel.OnValueChanged += (ushort prevValue, ushort newValue) =>
        {
            InitSlot(newValue);
        };
    }

    private void InitSlot(int slotId)
    {
        if (_activeBlock != null)
            _activeBlock.SetActive(false);
        var activatingBlock = _levels[_currentLevel.Value];
        activatingBlock.gameObject.SetActive(true);
        _activeBlock = activatingBlock.gameObject;
        var gettingHp = (ushort)activatingBlock.GetComponent<Block>().Hp;
        SetHpServerRpc(gettingHp);
        _startHp = gettingHp;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetLevelServerRpc(ushort value)
    {
        _currentLevel.Value = value;
    }
    
    [ContextMenu("Upgrade")]
    public void Upgrade()
        => SetLevelServerRpc((ushort)(_currentLevel.Value + 1));

    public bool CanBeRepaired()
    {
        int damagingPercent = _startHp / Hp.Value;
        List<InventoryCell> cells = new List<InventoryCell>();
        foreach (var cell in NeededCellsForPlace)
            cells.Add(new InventoryCell( cell.Item, cell.Count / damagingPercent));
        return false;
        // return InventorySlotsContainer.singleton.ItemsAvaliable(cells);
    }

    public void TryRepair()
    {
        if (!CanBeRepaired()) return;
        SetHpServerRpc(_startHp);
    }

    public bool CanBeUpgraded()
        => _currentLevel.Value < _levels.Count - 1;

    [ServerRpc(RequireOwnership = false)]
    private void SetHpServerRpc(ushort value)
    {
        Hp.Value = value;
        if (Hp.Value <= 0)
            Destroy(gameObject);
    }

    public void GetDamage(int damage)
    {
        int hp = Hp.Value - damage;
        SetHpServerRpc((ushort)hp);
    }

    public Block GetUpgradingBlock()
    {
        if (_currentLevel.Value == _levels.Count - 1)
            return _levels[_currentLevel.Value];
        return _levels[_currentLevel.Value + 1];
    }
}