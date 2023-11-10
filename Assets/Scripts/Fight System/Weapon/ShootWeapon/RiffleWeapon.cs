using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    [RequireComponent(typeof(WeaponSoundPlayer))]
    public class RiffleWeapon : BaseShootingWeapon
    {
        [Header("In Game Init")] 
        private int startingAmmoCount = 100;

        private LongRangeWeaponInventoryItemDisplayer inventoryItemDisplayer;
        
        private void Start()
        {
            Reload();
            canShoot = true;
            currentAmmoCount = startingAmmoCount;
        }

        private void Update()
        {
            Recoil.UpdateRecoil(2f);
        }

        public override void Attack()
        {
            if (!CanShoot() || currentAmmoCount <= 0) return;
            RaycastHit hit;
            SoundPlayer.PlayShot();
            MinusAmmo();
            Recoil.ApplyRecoil(Weapon.RecoilX, Weapon.RecoilY, Weapon.RecoilZ);
            StartCoroutine(DisplayFlameEffect()); // Start the coroutine
            if (Physics.Raycast(AmmoSpawnPoint.position, AmmoSpawnPoint.forward, out hit, Weapon.Range, TargetMask))
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