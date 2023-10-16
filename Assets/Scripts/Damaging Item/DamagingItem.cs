using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class DamagingItem : NetworkBehaviour, IDamagable
{
   [SerializeField] private NetworkVariable<int> _currentHp = new(50, NetworkVariableReadPermission.Everyone,
      NetworkVariableWritePermission.Owner);
   [SerializeField] private float _spawningYOffset = 1f;
   [SerializeField] private List<LootBarrelCell> _loot = new List<LootBarrelCell>();

   private int _cachedHp;

   private void Start()
   {
      transform.tag = "DamagingItem";
      _cachedHp = _currentHp.Value;
   }

   private void SpawnLootCell(LootBarrelCell cell)
   {
      int rand = Random.Range(cell.MinimalCount, cell.MaximalCount);
      var fixedPos = transform.position;
      fixedPos.y += _spawningYOffset;
      InstantiatingItemsPool.sigleton.SpawnObjectServerRpc(cell.Item.Id, rand, fixedPos);
   }
   
   private void Destroy()
   {
      foreach (var cell in _loot)
         SpawnLootCell(cell);
      Destroy(gameObject);
   }
   
   [ServerRpc]
   private void GetDamageServerRpc(int damage)
   {
      _currentHp.Value -= damage;
      if(_currentHp.Value <= 0)
         Destroy();
   }

   public ushort GetHp()
      => (ushort)_currentHp.Value;

   public int GetMaxHp()
      => _cachedHp;

   public void GetDamage(int damage)
   {
      GetDamageServerRpc(damage);
   }
}