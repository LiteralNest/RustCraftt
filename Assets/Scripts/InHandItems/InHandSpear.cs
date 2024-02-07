using FightSystem.Weapon.Melee;
using UnityEngine;

namespace InHandItems
{
    public class InHandSpear : MonoBehaviour
    {
        [SerializeField] private MeleeShootingWeapon _meleeShootingWeapon;
        [SerializeField] private WeaponThrower _weaponThrower;

        public void Gather()
            => _meleeShootingWeapon.Damage();
        
        public void EndThrow()
            => _weaponThrower.EndThrow();
    }
}