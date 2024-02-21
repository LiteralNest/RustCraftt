﻿using FightSystem.Weapon.Melee;
using UnityEngine;

namespace InHandItems
{
    public class InHandSpear : MonoBehaviour
    {
        [SerializeField] private MeleeShootingWeapon _meleeShootingWeapon;
        [SerializeField] private WeaponThrower _weaponThrower;

        public void Gather()
        {
            if (_meleeShootingWeapon.gameObject.activeSelf)
                _meleeShootingWeapon.Damage();
        }

        public void StartThrow()
        {
            if(_meleeShootingWeapon.gameObject.activeSelf)
                _weaponThrower.EndThrow();
        }
        
        public void EndThrow()
        {
            if(_meleeShootingWeapon.gameObject.activeSelf)
                _weaponThrower.EndThrow();
        }
    }
}