using Fight_System.Weapon.ShootWeapon.Ammo;
using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    public class Bow : BaseShootingWeapon
    {
        [SerializeField] private Arrow _arrowPrefab;
        [SerializeField] private float _arrowForce;

        private Arrow _currentArrow; // Added a field to store the current arrow instance

        private void Start()
        {
            base.Start();
            currentAmmoCount = 100;
            CreateArrow(); // Instantiate the arrow on start
        }

        public override void Attack(bool value)
        {
            if (CanShoot() && value)
            {
                ShootArrow();
            }
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

            var force = AmmoSpawnPoint.TransformDirection(Vector3.forward * _arrowForce);
            _currentArrow.ArrowFly(force); // Apply force and torque to the arrow

            MinusAmmo();
            StartCoroutine(WaitBetweenShootsRoutine());
        }
    }
}