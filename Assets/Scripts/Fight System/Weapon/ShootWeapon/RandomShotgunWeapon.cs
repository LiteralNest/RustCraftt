using Inventory_System.Inventory_Items_Displayer;
using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
   
    [RequireComponent(typeof(WeaponSoundPlayer))]
    public class RandomShotgunWeapon : BaseShootingWeapon
    {
        [SerializeField] private WeaponAim _weaponAim;

        [Header("Shotgun Settings")]
        [SerializeField] private int _pelletCount = 12;
        [SerializeField] private float _spreadRadiusNoFocus = 0.5f;
        [SerializeField] private float _spreadRadiusFocus = 0.1f;
        [SerializeField, Range(1, 6)] private int _shotProbability = 1;

        private LongRangeWeaponItemDisplayer _inventoryItemDisplayer;
        private Vector3[] _spreadOffsets;

        [ContextMenu("Shot")]
        private void TestShot() => Attack();

        public override void Attack()
        {
            if (!CanShoot() || currentAmmoCount <= 0 || !RandomShotSucceeded()) return;

            SoundPlayer.PlayShot();
            MinusAmmo();

            var spawnPoint = AmmoSpawnPoint.position;
            var shootDirection = transform.forward;
            StartCoroutine(DisplayFlameEffect()); // Start the coroutine
            SpreadShots(spawnPoint, shootDirection, _weaponAim.IsAiming ? _spreadRadiusFocus : _spreadRadiusNoFocus);
            AdjustRecoil();
            StartCoroutine(WaitBetweenShootsRoutine());
        }

        private bool RandomShotSucceeded()
        {
            return Random.Range(1, 7) <= _shotProbability;
        }

        private void SpreadShots(Vector3 spawnPoint, Vector3 shootDirection, float radius)
        {
            var angleStep = 360f / _pelletCount;

            for (var i = 0; i < _pelletCount; i++)
            {
                var angle = i * angleStep;

                var spreadOffset = Quaternion.AngleAxis(angle, shootDirection) * (Vector3.up * radius);
                var spreadDirection = (shootDirection + spreadOffset).normalized;

                var randomSpreadOffset = Random.insideUnitCircle * radius;

                var randomSpreadOffset3D = new Vector3(randomSpreadOffset.x, randomSpreadOffset.y, 0f);

                var shootRay = new Ray(spawnPoint, (spreadDirection + randomSpreadOffset3D).normalized);

                var raycast = Physics.Raycast(shootRay, out var hit, Weapon.Range, TargetMask);

                if (raycast)
                {
                    SpawnTrail(hit.point);
                    TryDamage(hit);
                    DisplayHit(hit);
                }
                else
                {
                    SpawnTrail(AmmoSpawnPoint.transform.forward * 10f);
                }
            }
        }
    }
}