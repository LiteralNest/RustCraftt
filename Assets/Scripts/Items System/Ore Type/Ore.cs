using System.Collections.Generic;
using System.Threading.Tasks;
using Items_System.Ore_Type;
using Unity.Netcode;
using UnityEngine;

public class Ore : NetworkBehaviour
{
    [Header("Start init")]
    [SerializeField] protected ushort _hp = 100;
    [SerializeField] protected NetworkVariable<ushort> _currentHp = new(100, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    [SerializeField] private float _recoveringSpeed;
    [SerializeField] protected List<OreSlot> _resourceSlots = new List<OreSlot>();
    [SerializeField] private List<Renderer> _renderers;

    public bool Recovering { get; protected set; } = false;

    protected void Start()
    {
        _currentHp.Value = _hp;
        _currentHp.OnValueChanged += (ushort prevValue, ushort newValue) =>
        {
            CheckHp();
        };
    }
    
    protected void AddResourcesToInventory()
    {
        foreach(var slot in _resourceSlots)
            InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(slot.Resource, Random.Range(slot.CountRange.x, slot.CountRange.y + 1));
    }
    
    private async void CheckHp()
    {
        if (_currentHp.Value > 0) return;
        await Destroy();
        if(NetworkManager.Singleton.IsServer)
            _currentHp.Value = _hp;
    }
    
    private void TurnRenderers(bool value)
    {
        foreach (var renderer in _renderers)
            renderer.enabled = value;
    }
    
    private async Task Recover()
    {
        Recovering = true;
        await Task.Delay((int)(_recoveringSpeed * 1000));
        Recovering = false;
        TurnRenderers(true);
    }
    
    protected async Task Destroy()
    {
        TurnRenderers(false);
        await Recover();
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void MinusHpServerRpc()
    {
        _currentHp.Value--;
    }
}
