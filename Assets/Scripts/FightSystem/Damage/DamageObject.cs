using Items_System.Items.Abstract;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Damage
{
    public class DamageObject : NetworkBehaviour
    {
        [SerializeField] private Item _damageItem;
        [SerializeField] private int _damage = 25;
        private bool _damaged;

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer || _damaged) return;
            if (other.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                _damaged = true;
                damagable.GetDamageOnServer(_damage);
                enabled = false;
            }

            else if (other.gameObject.TryGetComponent<IBuildingDamagable>(out var buildingDamagable))
            {
                _damaged = true;
                enabled = false;
                buildingDamagable.GetDamageOnServer(_damageItem.Id);
            }
        }
    }
}