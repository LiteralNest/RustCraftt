using Cysharp.Threading.Tasks;
using FightSystem.Weapon.Melee;
using UnityEngine;

namespace FightSystem.Weapon
{
    public class SpearBalisticTrajectoryTester : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private GameObject _throwingObjPrefab;
        private const float SpawnDelay = 1f;

        private bool _canShoot = true;

        private GameObject _instance;

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
            _instance = null;
            _canShoot = true;
        }

        private void Shoot()
        {
            _instance = Instantiate(_throwingObjPrefab, _spawnTransform.position, _throwingObjPrefab.transform.rotation);
            _instance.GetComponent<ThrowingWeapon>().Throw(2);
        }
    }
}