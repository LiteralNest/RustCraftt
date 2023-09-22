using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class LootBarrel : NetworkBehaviour, IDamagable
{
   [SerializeField] private NetworkVariable<int> _currentHp = new(50, NetworkVariableReadPermission.Everyone,
      NetworkVariableWritePermission.Owner);
   
   [SerializeField] private List<LootBarrelCell> _loot = new List<LootBarrelCell>();

   private void SpawnLootCell(LootBarrelCell cell)
   {
      int rand = Random.Range(cell.MinimalCount, cell.MaximalCount);
      InstantiatingItemsPool.sigleton.SpawnObjectServerRpc(cell.Item.Id, rand, transform.position);
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
   
   public void GetDamage(int damage)
   {
      GetDamageServerRpc(damage);
   }
}