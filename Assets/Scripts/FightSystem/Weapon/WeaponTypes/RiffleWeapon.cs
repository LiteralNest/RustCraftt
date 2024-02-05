using Inventory_System.Inventory_Items_Displayer;
using Player_Controller;
using UnityEngine;

namespace FightSystem.Weapon.WeaponTypes
{
    public class RiffleWeapon : BaseShootingWeapon
    {
        private LongRangeWeaponItemDisplayer _inventoryItemDisplayer;

        protected override void Attack()
        {
            if (!CanShoot() || CurrentAmmoCount <= 0) return;
            base.Attack();
            MinusAmmo();

            var ray = new Ray(AmmoSpawnPoint.position, AmmoSpawnPoint.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, Weapon.Range, TargetMask))
            {
                ShotEffectSpawner.SpawnTrailServerRpc(PlayerNetCode.Singleton.GetClientId(), _bulletSpeed, hit.point);
                TryDamage(hit);
                DisplayHit(hit);
            }
            else
                ShotEffectSpawner.SpawnTrailServerRpc(PlayerNetCode.Singleton.GetClientId(), _bulletSpeed,
                    AmmoSpawnPoint.position + AmmoSpawnPoint.forward * Weapon.Range);

            AdjustRecoil();
            StartCoroutine(WaitBetweenShootsRoutine());
        }
    }
}