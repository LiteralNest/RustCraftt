using System.Collections;
using Events;
using FightSystem.Damage;
using FightSystem.Weapon.ShootWeapon;
using FightSystem.Weapon.ShootWeapon.Sway;
using FightSystem.Weapon.ShootWeapon.TrailSystem;
using InHandItems.InHandAnimations.Weapon;
using InHandItems.InHandViewSystem;
using Items_System.Items.Weapon;
using Player_Controller;
using Sound_System;
using Sound_System.FightSystem.Damage;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace FightSystem.Weapon.WeaponTypes
{
    [RequireComponent(typeof(WeaponSway))]
    [RequireComponent(typeof(AimSway))]
    public abstract class BaseShootingWeapon : NetworkBehaviour, IViewable
    {
        private const string ViewName = "Weapon/View/LongRangeWeaponView";

        [Header("Main Parameters")] [SerializeField]
        protected LayerMask TargetMask;

        [SerializeField] protected int _bulletSpeed = 100;

        [Header("Attached Components")] [SerializeField]
        protected GameObject Decal;

        [SerializeField] private RifleAnimator _weaponAnimator;
        [SerializeField] private Transform _swayTransform;
        [SerializeField] protected ShootingWeapon Weapon;
        [SerializeField] protected Transform AmmoSpawnPoint;
        [SerializeField] protected GameObject ImpactEffect;
        [SerializeField] private NetworkSoundPlayer _soundPlayer;

        [FormerlySerializedAs("TrailSpawner")] [SerializeField]
        protected ShotEffectSpawner ShotEffectSpawner;

        [Header("Sound")] [SerializeField] private AudioClip _shotSound;

        [Header("Aim")] [SerializeField] private Transform _aimPosition;
        [SerializeField] private Transform _aimingTransform;

        [Header("Animation")] [SerializeField] private AnimationClip _reloadAnim;

        private WeaponSway _weaponSway;
        private AimSway _aimSway;

        private WeaponRecoil _recoil;
        protected WeaponAim WeaponAim;
        protected LongRangeInHandView InHandView;

        protected int CurrentAmmoCount;
        private bool _canShoot;
        private bool _isShooting;
        private readonly float _timeBetweenShots = 0f;
        private bool _isReloading = false;

        protected bool ViewAssign;

        private void OnEnable()
        {
            _isReloading = false;
            if (WeaponAim != null)
                WeaponAim.UnScope();
            if (InventoryHandler.singleton && InventoryHandler.singleton.ActiveSlotDisplayer)
                CurrentAmmoCount = InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell.Ammo;
            TryDisplayReload();
            TryDisplayAttack();
            GlobalEventsContainer.InventoryDataChanged += TryDisplayReload;
        }

        private void OnDisable()
        {
            GlobalEventsContainer.InventoryDataChanged -= TryDisplayReload;
        }

        protected void Start()
        {
            _weaponSway = GetComponent<WeaponSway>();
            _weaponSway.Init(_swayTransform);
            _aimSway = GetComponent<AimSway>();
            _aimSway.Init(_swayTransform);
            WeaponAim = new WeaponAim(_aimPosition, _aimingTransform, _weaponSway, _aimSway);
            _recoil = new WeaponRecoil(_swayTransform);
            _canShoot = true;
            if (!ViewAssign)
            {
                InHandView = Instantiate(Resources.Load<LongRangeInHandView>(ViewName), transform);
                InHandView.Init(this);
            }
        }

        private void Update()
        {
            if (_isShooting && CanShoot())
                Attack();
            _recoil.UpdateRecoil(3f);
            WeaponAim.UpdateSway();
        }

        public void SetCanShot(bool value)
            => _canShoot = value;

        protected virtual void Attack()
        {
            if (_weaponAnimator)
                _weaponAnimator.PlayShot();
            ShotEffectSpawner.DisplayEffectServerRpc();
            _soundPlayer.PlayOneShotFromClient(_shotSound);
        }

        public void Reload()
        {
            if (_isReloading) return;
            _weaponAnimator.PlayReload();
            _isReloading = true;
            var addingAmmo = InventoryHandler.singleton.CharacterInventory.GetItemCount(Weapon.Ammo.Id);
            if (addingAmmo <= 0)
                return;
            if (addingAmmo > Weapon.MagazineCount - CurrentAmmoCount)
                addingAmmo = Weapon.MagazineCount - CurrentAmmoCount;
            StartCoroutine(ReloadCoroutine(addingAmmo));
        }

        public void HandleShoot(bool value)
        {
            if (!value)
                _recoil.ResetRecoil();
            _isShooting = value;
        }

        public void Scope()
        {
            if (_isReloading) return;
            WeaponAim.SetScope();
        }

        protected bool CanShoot()
            => _canShoot && _timeBetweenShots <= 0 && CurrentAmmoCount > 0 && !_isReloading;

        private IEnumerator ReloadCoroutine(int count)
        {
            WeaponAim.UnScope(out bool wasUnScopped);
            InHandView.AssignReload(false);
            yield return new WaitForSeconds(_reloadAnim.length);
            if (wasUnScopped)
                WeaponAim.SetScope();
            CurrentAmmoCount += count;
            InventoryHandler.singleton.CharacterInventory.RemoveItem(Weapon.Ammo.Id, count);
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.SetCurrentAmmo(CurrentAmmoCount);
            InHandView.AssignAttack(true);
            _isReloading = false;
        }

        protected bool TryDamage(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent<IDamagable>(out var damagableObj))
            {
                StartCoroutine(CharacterUIHandler.singleton.DisplayHitRoutine());
                PlayerNetCode.Singleton.PlayerSoundsPlayer.PlaySoundLocal(damagableObj.GetPlayerDamageClip());
                damagableObj.GetDamage((int)(Weapon.Damage * Weapon.Ammo.MultiplyKoef));
                return true;
            }

            return false;
        }

        protected bool DisplayHit(RaycastHit hit)
        {
            if (hit.transform.GetComponent<Collider>().isTrigger) return false;
            var fire = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(fire, 2f);
            var decalObj = Instantiate(Decal, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(decalObj, 5);
            return true;
        }

        public void TryDisplayReload()
        {
            if (InventoryHandler.singleton == null || InventoryHandler.singleton.ActiveSlotDisplayer == null ||
                InHandView == null || CurrentAmmoCount >= Weapon.MagazineCount)
            {
                if (InHandView != null)
                    InHandView.AssignReload(false);
                return;
            }

            var addingAmmo = InventoryHandler.singleton.CharacterInventory.GetItemCount(Weapon.Ammo.Id);
            InHandView.AssignReload(addingAmmo > 0);
        }

        private void TryDisplayAttack()
        {
            if (InHandView == null) return;
            InHandView.AssignAttack(CurrentAmmoCount > 0);
        }

        protected void MinusAmmo()
        {
            CurrentAmmoCount--;
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.MinusCurrentAmmo(1);
            TryDisplayReload();
            TryDisplayAttack();
        }

        protected IEnumerator WaitBetweenShootsRoutine()
        {
            _canShoot = false;
            yield return new WaitForSeconds(Weapon.DelayBetweenShoots);
            _canShoot = true;
        }

        protected void AdjustRecoil()
        {
            var recoilX = Weapon.RecoilX;
            var recoilY = Weapon.RecoilY;
            var recoilZ = Weapon.RecoilZ;

            if (WeaponAim.IsAiming)
            {
                // Reduce recoil by half when aiming
                recoilX /= 4f;
                recoilY /= 4f;
                recoilZ /= 4f;

                _recoil.ApplyRecoil(recoilX, recoilY, recoilZ);
            }
            else
                _recoil.ApplyRecoil(recoilX, recoilY, recoilZ);
        }
    }
}