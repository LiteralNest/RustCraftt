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
            
            //PlayShot
            
            
            //
            MinusAmmo();
           

            var raycastedTargets =
                Physics.RaycastAll(AmmoSpawnPoint.position, AmmoSpawnPoint.forward, Weapon.Range, TargetMask);

            bool hitDisplayed = false;
            bool damaged = false;

            foreach (var hit in raycastedTargets)
            {
                if (!hitDisplayed)
                {
                    ShotEffectSpawner.SpawnTrailServerRpc(PlayerNetCode.Singleton.GetClientId(), _bulletSpeed, hit.point);
                    hitDisplayed = DisplayHit(hit);
                }

                if (!damaged)
                    damaged = TryDamage(hit);

                if (damaged) break;
            }
            
            if (!hitDisplayed)
                ShotEffectSpawner.SpawnTrailServerRpc(PlayerNetCode.Singleton.GetClientId(), _bulletSpeed, AmmoSpawnPoint.position + AmmoSpawnPoint.forward * Weapon.Range);

            AdjustRecoil();
            StartCoroutine(WaitBetweenShootsRoutine());
        }
    }
}