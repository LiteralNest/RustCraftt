using System.Collections.Generic;
using System.Threading.Tasks;
using Items_System.Ore_Type;
using Unity.Netcode;
using UnityEngine;

public class Ore : NetworkBehaviour
{
    [Header("Start init")]
    [SerializeField] protected int _hp = 100;
    [SerializeField] protected NetworkVariable<int> _currentHp = new(100, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    [SerializeField] private float _recoveringSpeed;
    [SerializeField] protected List<OreSlot> _resourceSlots = new List<OreSlot>();
    [SerializeField] private List<GameObject> _renderers;

    public bool Recovering { get; protected set; } = false;

    protected void Start()
    {
        _currentHp.Value = _hp;
        _currentHp.OnValueChanged += (int prevValue, int newValue) =>
        {
            CheckHp();
        };
    }
    
    protected void AddResourcesToInventory()
    {
        foreach(var slot in _resourceSlots)
            InventoryHandler.singleton.CharacterInventory.AddItemToDesiredSlotServerRpc(slot.Resource.Id, Random.Range(slot.CountRange.x, slot.CountRange.y + 1));
    }
    
    private void CheckHp()
    {
        if (_currentHp.Value > 0) return;
        DestroyObject();
    }
    
    protected void TurnRenderers(bool value)
    {
        foreach (var renderer in _renderers)
            renderer.SetActive(value);
    }
    
    private async Task Recover()
    {
        Recovering = true;
        await Task.Delay((int)(_recoveringSpeed * 1000));
        Recovering = false;
        TurnRenderers(true);
    }

    protected virtual void DestroyObject()
        => Destroy();
    
    protected async Task Destroy()
    {
        TurnRenderers(false);
        await Recover();
        if(NetworkManager.Singleton.IsServer)
            _currentHp.Value = _hp;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void MinusHpServerRpc()
    {
        if (_currentHp.Value <= 0) return;
        _currentHp.Value--;
    }
}
