using System;
using Fight_System.Weapon.ShootWeapon.Ammo;
using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    public class Bow : BaseShootingWeapon
    {
        [SerializeField] private Arrow _arrowPrefab;
        [SerializeField] private float _arrowForce;

        private Arrow _currentArrow;
        private Vector3 _force;
        protected new void Start()
        {
            base.Start();
            currentAmmoCount = 100;

            // Instantiate arrow at the start
            CreateArrow();
        }
        

        public override void Attack()
        {
            if (!CanShoot()) return;
            ShootArrow();
        }

        private void CreateArrow()
        {
            if (_arrowPrefab != null)
            {
                _currentArrow = Instantiate(_arrowPrefab, AmmoSpawnPoint.position, AmmoSpawnPoint.rotation);
                _currentArrow.transform.SetParent(AmmoSpawnPoint);
            }
        }

        private void ShootArrow()
        {
            if (_currentArrow == null)
            {
                CreateArrow();
            }

            _force = AmmoSpawnPoint.TransformDirection(Vector3.forward * _arrowForce);
            _currentArrow.ArrowFly(_force);
            _currentArrow.transform.SetParent(null);
            MinusAmmo();

            // Spawn a new arrow immediately after shooting
            CreateArrow();

            StartCoroutine(WaitBetweenShootsRoutine());
        }

        private void OnDrawGizmos()
        {
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(AmmoSpawnPoint.position, AmmoSpawnPoint.forward * _arrowForce);
            }
        }
    }
}