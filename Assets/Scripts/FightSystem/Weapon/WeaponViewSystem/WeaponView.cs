using FightSystem.Weapon.ShootWeapon;
using UnityEngine;

namespace FightSystem.Weapon.WeaponViewSystem
{
    public abstract class WeaponView : MonoBehaviour
    {
        public abstract void Init(BaseShootingWeapon weapon);
    }
}