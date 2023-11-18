using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Building_System.Upgrading;
using Unity.Netcode;
using UnityEngine;

public class BuildingBlock : NetworkBehaviour, IDamagable, IHammerInteractable
{
    [SerializeField] private List<Block> _levels;

    private NetworkVariable<ushort> _hp = new(100, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public Block CurrentBlock => _levels[_currentLevel.Value];
    [field: SerializeField] public StructureConnector BuildingConnector { get; private set; }

    [Tooltip("In Seconds")] [SerializeField]
    private float _destroyingTime = 600f;

    private DateTime _placingTime;

    public ushort StartHp => _startHp;
    private ushort _startHp;

    private NetworkVariable<ushort> _currentLevel = new(0, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    private List<InventoryCell> _cellsForRepairing = new List<InventoryCell>();
    
    private GameObject _activeBlock;

    public override void OnNetworkSpawn()
    {
        _placingTime = DateTime.Now;
        InitSlot(_currentLevel.Value);
        _currentLevel.OnValueChanged += (ushort prevValue, ushort newValue) => { InitSlot(newValue); };
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

    public List<InventoryCell> GetNeededCellsForPlacing()
        => _levels[_currentLevel.Value].CellForPlace;

    [ServerRpc(RequireOwnership = false)]
    private void SetLevelServerRpc(ushort value)
    {
        _currentLevel.Value = value;
    }


    [ServerRpc(RequireOwnership = false)]
    private void SetHpServerRpc(ushort value)
    {
        _hp.Value = value;
        if (_hp.Value <= 0)
        {
            if (IsServer)
                Destroy();
        }
    }

    private async void DestroyAsync()
    {
        //Потрібно щоб OnTriggerExit зчитався
        transform.position = new Vector3(1000000, 1000000, 1000000);
        await Task.Delay(1000);
        if(gameObject == null) return;
        var networkObj = GetComponent<NetworkObject>();
        if (networkObj != null)
            networkObj.Despawn();
        Destroy(gameObject);
    }

    private bool MaxHp()
        => _hp.Value >= _startHp;
    
    public void RestoreHealth(int value)
    {
        int hp = _hp.Value + value;
        if(hp > _startHp)
            hp = _startHp;
        SetHpServerRpc((ushort)hp);
    }

    
    #region IHammerInteractable

    public bool CanBeRepaired()
    {
        if (MaxHp()) return false;
        int damagingPercent = 100 - (_hp.Value * 100 / _startHp);
        _cellsForRepairing.Clear();
        foreach (var cell in GetNeededCellsForPlacing())
            _cellsForRepairing.Add(new InventoryCell( cell.Item, cell.Count / damagingPercent));
        return InventoryHandler.singleton.CharacterInventory.EnoughMaterials(_cellsForRepairing);
    }

    public void Repair()
    {
        SetHpServerRpc(_startHp);
        InventoryHandler.singleton.CharacterInventory.RemoveItems(_cellsForRepairing);
        _cellsForRepairing.Clear();
    }

    public bool CanBeDestroyed()
    {
        var res = DateTime.Now - _placingTime;
        return res < TimeSpan.FromSeconds(_destroyingTime);
    }

    public void Destroy()
        => DestroyAsync();

    public void Shake()
    {
        
    }

    public bool CanBeUpgraded()
    {
        if(!MaxHp()) return false;
        int nextLevel = _currentLevel.Value + 1;
        if(nextLevel >= _levels.Count) return false;
        if (!InventoryHandler.singleton.CharacterInventory.EnoughMaterials(_levels[nextLevel].CellForPlace)) return false;
        return true;
    }

    public void Upgrade()
    {
        int nextLevel = _currentLevel.Value + 1;
        InventoryHandler.singleton.CharacterInventory.RemoveItems(_levels[nextLevel].CellForPlace);
        SetLevelServerRpc((ushort)nextLevel);
    }

    public bool CanBePickUp()
        => false;

    public void PickUp()
    {
        throw new System.NotImplementedException();
    }

    #endregion
    
    #region IDamagable

    public void GetDamage(int damage)
    {
        int hp = _hp.Value - damage;
        SetHpServerRpc((ushort)hp);
    }

    public ushort GetHp()
        => _hp.Value;

    public int GetMaxHp()
        => CurrentBlock.Hp;

    #endregion
}