using Inventory_System.Inventory_Items_Displayer;
using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    [RequireComponent(typeof(WeaponSoundPlayer))]
    public class ShotgunWeapon : BaseShootingWeapon
    {
        [SerializeField] private WeaponAim _weaponAim;
        
        [Header("Shotgun Settings")]
        [SerializeField] private int _pelletCount = 12;
        [SerializeField] private float _spreadRadiusNoFocus = 0.5f;
        [SerializeField] private float _spreadRadiusFocus = 0.1f;

        private LongRangeWeaponItemDisplayer _inventoryItemDisplayer;
        private Vector3[] _spreadOffsets;
        

        [ContextMenu("Shot")]
        private void TestShot() => Attack();

        public override void Attack()
        {
            if (!CanShoot() || currentAmmoCount <= 0) return;
            
            SoundPlayer.PlayShot();
            MinusAmmo();

            var spawnPoint = AmmoSpawnPoint.position;
            var shootDirection = transform.forward;
            StartCoroutine(DisplayFlameEffect()); // Start the coroutine
            SpreadShots(spawnPoint, shootDirection, _weaponAim.IsAiming ? _spreadRadiusFocus : _spreadRadiusNoFocus);
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
                    SpawnTrailServerRpc(hit.point);
                    TryDamage(hit);
                    DisplayHit(hit);
                }
                else
                {
                    SpawnTrailServerRpc(AmmoSpawnPoint.transform.forward * 10f);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var spawnPoint = AmmoSpawnPoint.position;
            var shootDirection = transform.forward;

            Gizmos.color = Color.green; 
            Gizmos.DrawLine(spawnPoint, spawnPoint + shootDirection * Weapon.Range);

            float angleStep = 360f / _pelletCount;

            for (int i = 0; i < _pelletCount; i++)
            {
                var angle = i * angleStep;
                var spreadAngleRad = angle * Mathf.Deg2Rad;

                var x = Mathf.Cos(spreadAngleRad);
                var y = Mathf.Sin(spreadAngleRad);

                var spreadOffset = new Vector3(x, y, 0) * _spreadRadiusNoFocus;

                var spreadDirection = (shootDirection + spreadOffset).normalized;
            
                var randomSpreadOffset = Random.insideUnitCircle * _spreadRadiusNoFocus;
            
                var randomSpreadOffset3D = new Vector3(randomSpreadOffset.x, randomSpreadOffset.y, 0f);

                Gizmos.DrawLine(spawnPoint, spawnPoint + (spreadDirection + randomSpreadOffset3D).normalized * Weapon.Range);
            }
        }
#endif
    }
}
