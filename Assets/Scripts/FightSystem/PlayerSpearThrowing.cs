using Events;
using FightSystem.Weapon.Melee;
using UnityEngine;

namespace FightSystem
{
    public class PlayerSpearThrowing : MonoBehaviour
    {
        private MeleeShootingWeapon _meleeShootingWeapon;

        private void OnEnable()
            => GlobalEventsContainer.WeaponMeleeObjectAssign += AssignMeleeWeaponObject;

        private void OnDisable()
            => GlobalEventsContainer.WeaponMeleeObjectAssign -= AssignMeleeWeaponObject;

        private void AssignMeleeWeaponObject(MeleeShootingWeapon value)
            => _meleeShootingWeapon = value;

        public void Scope(bool value)
        {
            if(!_meleeShootingWeapon) return;
            _meleeShootingWeapon.SetThrowingPosition(value);
        }
    }
}