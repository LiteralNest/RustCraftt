using Player_Controller;
using UnityEngine;

namespace FightSystem.Weapon.WeaponTypes
{
    public class ShotgunWeapon : BaseShootingWeapon
    {
        [Header("Shotgun Settings")] [SerializeField]
        private int _pelletCount = 12;

        [SerializeField] private float _spreadRadiusNoFocus = 0.5f;
        [SerializeField] private float _spreadRadiusFocus = 0.1f;
        
        private Vector3[] _spreadOffsets;

        protected override void Attack()
        {
            if (!CanShoot() || CurrentAmmoCount <= 0) return;

            base.Attack();
            
            //PlayShot
    
            MinusAmmo();

            var spawnPoint = AmmoSpawnPoint.position;
            var shootDirection = transform.forward;
            SpreadShots(spawnPoint, shootDirection, WeaponAim.IsAiming ? _spreadRadiusFocus : _spreadRadiusNoFocus);
            AdjustRecoil();
            StartCoroutine(WaitBetweenShootsRoutine());
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
                    ShotEffectSpawner.SpawnTrailServerRpc(PlayerNetCode.Singleton.GetClientId(), _bulletSpeed, hit.point);
                    TryDamage(hit);
                    DisplayHit(hit);
                }
                else
                {
                    ShotEffectSpawner.SpawnTrailServerRpc(PlayerNetCode.Singleton.GetClientId(), _bulletSpeed,
                        AmmoSpawnPoint.transform.forward * 10f);
                }
            }
        }
    }
}