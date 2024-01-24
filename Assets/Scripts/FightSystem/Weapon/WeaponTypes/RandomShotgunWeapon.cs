using System.Collections;
using InHandItems.InHandAnimations.Weapon;
using InHandItems.InHandViewSystem;
using Inventory_System.Inventory_Items_Displayer;
using Player_Controller;
using UnityEngine;

namespace FightSystem.Weapon.WeaponTypes
{
    public class RandomShotgunWeapon : BaseShootingWeapon
    {
        private const string EokaView = "Weapon/View/EokaView";

        [Header("Shotgun Settings")] [SerializeField]
        private int _pelletCount = 12;

        [SerializeField] private AnimationClip _missFireAnim;
        [SerializeField] private float _spreadRadiusNoFocus = 0.5f;
        [SerializeField] private float _spreadRadiusFocus = 0.1f;
        [SerializeField, Range(1, 100)] private int _shotProbability = 25;
        [SerializeField] private EokaAnimator _eokaAnimator;
        private LongRangeWeaponItemDisplayer _inventoryItemDisplayer;
        private Vector3[] _spreadOffsets;

        private bool _isMissingAnimation;
        private bool _missingFirePlayed;

        private bool _shooted;
        private EokaView _eokaView;

        private void Start()
        {
            ViewAssign = true;
            base.Start();
            _eokaView = Instantiate(Resources.Load<EokaView>(EokaView), transform);

            _eokaView.Init(this);
        }

        public void StartAttack()
        {
            _shooted = false;
            CurrentAmmoCount = 1;
            _eokaAnimator.PlayStartFire();
            HandleShoot(true);
            SetCanShot(true);
        }

        public void StopAttack()
        {
            if (_shooted || CanShoot()) return;
            _eokaAnimator.PlayStopFire();
            HandleShoot(false);
            SetCanShot(false);
        }

        private IEnumerator WaitMissAnimRoutine()
        {
            _isMissingAnimation = true;
            yield return new WaitForSeconds(_missFireAnim.length);
            _isMissingAnimation = false;
        }

        protected override void Attack()
        {
            if (_isMissingAnimation || !CanShoot() || CurrentAmmoCount <= 0) return;
            if (!RandomShotSucceeded())
            {
                if (!_missingFirePlayed)
                    _eokaAnimator.PlayMissFire();
                _missingFirePlayed = true;
                StartCoroutine(WaitMissAnimRoutine());
                return;
            }

            _missingFirePlayed = false;
            base.Attack();
            _eokaAnimator.PlayFire();
            SetCanShot(false);
            _shooted = true;
            //PlayShot

            MinusAmmo();

            var spawnPoint = AmmoSpawnPoint.position;
            var shootDirection = transform.forward;
            SpreadShots(spawnPoint, shootDirection, WeaponAim.IsAiming ? _spreadRadiusFocus : _spreadRadiusNoFocus);
            AdjustRecoil();
            StartCoroutine(WaitBetweenShootsRoutine());
        }

        private bool RandomShotSucceeded()
        {
            bool res = Random.Range(0, 100) <= _shotProbability;
            Debug.Log(res);
            return res;
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
                    ShotEffectSpawner.SpawnTrailServerRpc(PlayerNetCode.Singleton.GetClientId(), _bulletSpeed,
                        hit.point);
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