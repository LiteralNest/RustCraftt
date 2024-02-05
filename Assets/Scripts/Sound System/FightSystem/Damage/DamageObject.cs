using Sound_System.FightSystem.Damage;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Damage
{
   public class DamageObject : NetworkBehaviour
   {
      [SerializeField] private int _damage = 25;

      private void OnTriggerEnter(Collider other)
      {
         if (!other.gameObject.TryGetComponent<IDamagable>(out var damagable)) return;
         if(IsServer)
            damagable.GetDamage(_damage);
      }
   }
}
