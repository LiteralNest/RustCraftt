using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    [RequireComponent(typeof(WeaponSoundPlayer))]
    public class ShotgunWeapon : BaseShootingWeapon
    {
        [Header("Shotgun Settings")]
        [SerializeField] private int _pelletCount = 12;
        [SerializeField] private float _spreadRadius = 0.5f; 

        [Header("In Game Init")] 
        private readonly int _startingAmmoCount = 100;

        private LongRangeWeaponInventoryItemDisplayer _inventoryItemDisplayer;
        private Vector3[] _spreadOffsets;

        private void Start()
        {
            Reload();
            canShoot = true;
            currentAmmoCount = _startingAmmoCount;
        }

        private void Update()
        {
            Recoil.UpdateRecoil(2f);
        }

        [ContextMenu("Shot")]
        private void TestShot() => Attack(true);

        public override void Attack(bool value)
        {
            if (!CanShoot() || currentAmmoCount <= 0) return;
            SoundPlayer.PlayShot();
            MinusAmmo();

            var spawnPoint = AmmoSpawnPoint.position;
            var shootDirection = transform.forward;

            SpreadShots(spawnPoint, shootDirection);
            StartCoroutine(WaitBetweenShootsRoutine());
        }

        private void SpreadShots(Vector3 spawnPoint, Vector3 shootDirection)
        {
            var angleStep = 360f / _pelletCount;

            for (var i = 0; i < _pelletCount; i++)
            {
                var angle = i * angleStep;
                var spreadAngleRad = angle * Mathf.Deg2Rad;

                var x = Mathf.Cos(spreadAngleRad);
                var z = Mathf.Sin(spreadAngleRad);

                var spreadOffset = new Vector3(0f, x, z) * _spreadRadius; 

                var spreadDirection = (shootDirection + spreadOffset).normalized;

                var randomSpreadOffset = Random.insideUnitCircle * _spreadRadius;
        
                var randomSpreadOffset3D = new Vector3(0f, randomSpreadOffset.x, randomSpreadOffset.y);

                var shootRay = new Ray(spawnPoint, (spreadDirection + randomSpreadOffset3D).normalized);

                Recoil.ApplyRecoil(Weapon.RecoilX, Weapon.RecoilY, Weapon.RecoilZ);

                if (Physics.Raycast(shootRay, out var hit, Weapon.Range, TargetMask))
                {
                    TryDamage(hit);
                    DisplayHit(hit);
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
                var z = Mathf.Sin(spreadAngleRad);

                var spreadOffset = new Vector3(0f, x, z) * _spreadRadius;

                var spreadDirection = (shootDirection + spreadOffset).normalized;
            
                var randomSpreadOffset = Random.insideUnitCircle * _spreadRadius;
            
                var randomSpreadOffset3D = new Vector3(0f, randomSpreadOffset.x, randomSpreadOffset.y);

                Gizmos.DrawLine(spawnPoint, spawnPoint + (spreadDirection + randomSpreadOffset3D).normalized * Weapon.Range);
            }
        }
#endif
    }
}
