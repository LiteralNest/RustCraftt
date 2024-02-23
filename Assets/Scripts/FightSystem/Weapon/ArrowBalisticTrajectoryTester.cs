using Cysharp.Threading.Tasks;
using FightSystem.Weapon.ShootWeapon.Ammo;
using UnityEngine;

namespace FightSystem.Weapon
{
    public class ArrowBalisticTrajectoryTester : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private GameObject _throwingObjPrefab;
        private const float SpawnDelay = 1f;

        private bool _canShoot = true;

        private GameObject _arrow;

        private void Update()
        {
            if ( _canShoot)
            {
                ShootWithDelay().Forget();
            }
        }

        private async UniTaskVoid ShootWithDelay()
        {
            _canShoot = false;
            await UniTask.Delay((int)(SpawnDelay * 1000));
            Shoot();
            _arrow = null;
            _canShoot = true;
        }

        private void Shoot()
        {
            _arrow = Instantiate(_throwingObjPrefab, _spawnTransform.position, _throwingObjPrefab.transform.rotation);
            _arrow.GetComponent<Arrow>().ArrowFly();
        }
    }
}
