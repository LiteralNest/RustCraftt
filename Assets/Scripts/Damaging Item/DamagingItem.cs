using System.Collections.Generic;
using Sound_System;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class DamagingItem : NetworkBehaviour, IDamagable
{
    [SerializeField] private NetworkVariable<int> _currentHp = new(50, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [SerializeField] private float _spawningYOffset = 1f;
    [SerializeField] private List<LootCell> _loot = new List<LootCell>();

    [SerializeField] private List<Renderer> _renderers;
    [SerializeField] private BoxCollider _boxCollider;
    [Header("Sound")] 
    [SerializeField] private NetworkSoundPlayer _soundPlayer;
    [SerializeField] private AudioClip _damagingSound;
    [SerializeField] private AudioClip _destroyingSound;
    private int _cachedHp;
    private bool _canGetDamage;

    private void Start()
    {
        _canGetDamage = true;
        transform.tag = "DamagingItem";
        _cachedHp = _currentHp.Value;
    }

    public override void OnNetworkSpawn()
    {
        _currentHp.OnValueChanged += (int prevValue, int newValue) => { CheckHp(newValue); };
    }

    private void SpawnLootCell(LootCell cell)
    {
        int rand = Random.Range(cell.MinimalCount, cell.MaximalCount);
        var fixedPos = transform.position;
        fixedPos.y += _spawningYOffset;
        InstantiatingItemsPool.sigleton.SpawnObjectServerRpc(cell.Item.Id, rand, fixedPos);
    }

    public void Destroy()
    {
        if (IsServer)
        {
            _soundPlayer.PlayOneShot(_destroyingSound);
            foreach (var cell in _loot)
                SpawnLootCell(cell);
        }

        _boxCollider.enabled = false;
        _canGetDamage = false;
        EnableRenderers(false);
    }

    public void Shake()
    {
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetDamageServerRpc(int damage, bool value)
    {
        _currentHp.Value -= damage;
        _soundPlayer.PlayOneShot(_damagingSound);
    }

    public ushort GetHp()
        => (ushort)_currentHp.Value;

    public int GetMaxHp()
        => _cachedHp;

    private void EnableRenderers(bool value)
    {
        foreach (var renderer in _renderers)
            renderer.enabled = value;
    }

    public void GetDamage(int damage, bool playSound = true)
    {
        if (!_canGetDamage) return;
        GetDamageServerRpc(damage, playSound);
    }

    private void CheckHp(int value)
    {
        if (value > 0) return;
        Destroy();
    }
}