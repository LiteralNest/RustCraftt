using System.Collections.Generic;
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
      _currentHp.OnValueChanged += (int prevValue, int newValue) =>
      {
         CheckHp(newValue);
      };
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
         foreach (var cell in _loot)
            SpawnLootCell(cell);
      }

      _boxCollider.enabled = false;
      _canGetDamage = false;
      EnableRenderers(false);
   }
   
   [ServerRpc(RequireOwnership = false)]
   private void GetDamageServerRpc(int damage)
   {
      _currentHp.Value -= damage;
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
   
   public void GetDamage(int damage)
   {
      if(!_canGetDamage) return;
      GetDamageServerRpc(damage);
   }

   private void CheckHp(int value)
   {
      if (value > 0) return;
      Destroy();
   }
}