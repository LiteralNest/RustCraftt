using Inventory_System.Inventory_Items_Displayer;
using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    [RequireComponent(typeof(WeaponSoundPlayer))]
    public class RiffleWeapon : BaseShootingWeapon
    {
        [Header("In Game Init")]
        private int _startingAmmoCount = 100;

        private LongRangeWeaponItemDisplayer _inventoryItemDisplayer;
        
        private void Start()
        {
            Reload();
            canShoot = true;
            currentAmmoCount = _startingAmmoCount;
        }

     
        public override void Attack()
        {
            if (!CanShoot() || currentAmmoCount <= 0) return; 

            SoundPlayer.PlayShot();
            MinusAmmo();
            AdjustRecoil();
            StartCoroutine(DisplayFlameEffect());
            
            if (Physics.Raycast(AmmoSpawnPoint.position, AmmoSpawnPoint.forward, out var hit, Weapon.Range, TargetMask))
            {
                TryDamage(hit);
                DisplayHit(hit);
            }
            StartCoroutine(WaitBetweenShootsRoutine());
        }
        
        private void OnDrawGizmos()
        {
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(AmmoSpawnPoint.position, AmmoSpawnPoint.forward * 50f);
            }
        }
    }
}