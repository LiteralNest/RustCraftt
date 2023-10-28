using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class Ore : NetworkBehaviour
{
    [Header("Start init")]
    [SerializeField] protected ushort _hp = 100;
    [SerializeField] protected NetworkVariable<ushort> _currentHp = new(100, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    [SerializeField] private float _recoveringSpeed;
    public Resource TargetResource => _targetResource;
    [SerializeField] protected Resource _targetResource;
    [SerializeField] protected Vector2Int _addingCount = new Vector2Int(1,2);
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
