using FightSystem.Weapon.ShootWeapon.Ammo;
using FightSystem.Weapon.WeaponTypes;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon
{
    public class Bow : BaseShootingWeapon
    {
        [SerializeField] private Arrow _arrowPrefab;
        [SerializeField] private float _arrowForce;

        private Arrow _currentArrow;
        private Vector3 _force;

        protected new void Start()
        {
            CurrentAmmoCount = 100;

            // Instantiate arrow at the start
            CreateArrow();
        }


        protected override void Attack()
        {
            base.Attack();
            // if (!CanShoot() || currentAmmoCount <= 0) return;
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

#if UnityEditor
        private void OnDrawGizmos()
        {
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(AmmoSpawnPoint.position, AmmoSpawnPoint.forward * _arrowForce);
            }
        }

#endif
    }
}