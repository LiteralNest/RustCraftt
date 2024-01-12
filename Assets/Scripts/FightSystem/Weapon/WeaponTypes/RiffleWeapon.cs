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
            
            SoundPlayer.PlayShot();
            MinusAmmo();
            StartCoroutine(DisplayFlameEffect());

            var raycastedTargets =
                Physics.RaycastAll(AmmoSpawnPoint.position, AmmoSpawnPoint.forward, Weapon.Range, TargetMask);

            bool hitDisplayed = false;
            bool damaged = false;

            foreach (var hit in raycastedTargets)
            {
                if (!hitDisplayed)
                {
                    _trailSpawner.SpawnTrailServerRpc(PlayerNetCode.Singleton.GetClientId(), _bulletSpeed, hit.point);
                    hitDisplayed = DisplayHit(hit);
                }

                if (!damaged)
                    damaged = TryDamage(hit);

                if (damaged) break;
            }

            AdjustRecoil();
            StartCoroutine(WaitBetweenShootsRoutine());
        }
    }
}