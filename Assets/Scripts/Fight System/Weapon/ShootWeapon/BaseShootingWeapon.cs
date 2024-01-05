using System.Collections;
using Fight_System.Weapon.ShootWeapon.TrailSystem;
using Items_System.Items.Weapon;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace Fight_System.Weapon.ShootWeapon
{
    public abstract class BaseShootingWeapon : NetworkBehaviour, IWeapon
    {
        [SerializeField] protected WeaponAim WeaponAim;
        [SerializeField] private bool canBeReloaded = false;

        [Header("Shooting Mode")] [SerializeField]
        private bool _isSingle;

        [SerializeField] protected WeaponRecoil Recoil;
        [SerializeField] protected WeaponSoundPlayer SoundPlayer;
        [SerializeField] protected Transform AmmoSpawnPoint;
        [SerializeField] protected GameObject ImpactEffect;
        [SerializeField] protected VisualEffect FlameEffect;
        [SerializeField] protected float FlameEffectDuration;
        [SerializeField] protected GameObject Decal;
        [SerializeField] protected LayerMask TargetMask;
        [SerializeField] protected ShootingWeapon Weapon;

        [Header("Trail Settings")] [SerializeField]
        protected TrailSpawner _trailSpawner;

        [SerializeField] protected int _bulletSpeed = 100;

        public bool IsSingle => _isSingle;
        protected int CurrentAmmoCount;
        protected bool canShoot;
        private float _timeBetweenShots = 0f;
        private bool _isReloading = false;

        protected void Start()
            => canShoot = true;

        private void Update()
        {
            Recoil.UpdateRecoil(3f);
        }

        protected bool CanShoot()
        {
            return canShoot && _timeBetweenShots <= 0 && CurrentAmmoCount > 0 && !_isReloading;
        }

        private void OnEnable()
        {
            CharacterUIHandler.singleton.ActivateScope(true);
            TryDisplayReload();
            GlobalEventsContainer.WeaponObjectAssign?.Invoke(this);
            GlobalEventsContainer.InventoryDataChanged += TryDisplayReload;
        }

        private void OnDisable()
        {
            GlobalEventsContainer.InventoryDataChanged -= TryDisplayReload;
            CharacterUIHandler.singleton.ActivateScope(false);
            CharacterUIHandler.singleton.ActivateReloadingButton(false);
            CharacterUIHandler.singleton.ActivateAttackButton(false);
            GlobalEventsContainer.WeaponObjectAssign?.Invoke(null);
        }

        public void Init()
        {
            CurrentAmmoCount = InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.GetCurrentAmmo();
            if (CurrentAmmoCount > 0) CharacterUIHandler.singleton.ActivateAttackButton(true);
        }

        public virtual void Attack()
        {
        }

        public virtual bool CanReload() => canBeReloaded;

        public virtual void Reload()
        {
            if (_isReloading) return;
            _isReloading = true;
            var addingAmmo = InventoryHandler.singleton.CharacterInventory.GetItemCount(Weapon.Ammo.Id);
            if (addingAmmo <= 0)
                return;
            if (addingAmmo > Weapon.MagazineCount)
                addingAmmo = Weapon.MagazineCount;
            StartCoroutine(ReloadCoroutine(addingAmmo));
        }

        private IEnumerator ReloadCoroutine(int count)
        {
            yield return new WaitForSeconds(1f);
            CurrentAmmoCount += count;
            InventoryHandler.singleton.CharacterInventory.RemoveItem(Weapon.Ammo.Id, count);
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.SetCurrentAmmo(CurrentAmmoCount);
            CharacterUIHandler.singleton.ActivateAttackButton(true);
            _isReloading = false;
        }

        protected bool TryDamage(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent<IDamagable>(out var damagableObj))
            {
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

        private void TryDisplayReload()
        {
            var addingAmmo = InventoryHandler.singleton.CharacterInventory.GetItemCount(Weapon.Ammo.Id);
            if (addingAmmo <= 0) return;
            CharacterUIHandler.singleton.ActivateReloadingButton(true);
        }

        protected void MinusAmmo()
        {
            TryDisplayReload();
            CurrentAmmoCount--;
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.MinusCurrentAmmo(1);
            if (CurrentAmmoCount <= 0)
                CharacterUIHandler.singleton.ActivateAttackButton(false);
        }

        protected IEnumerator DisplayFlameEffect()
        {
            FlameEffect.Play();
            yield return new WaitForSeconds(FlameEffectDuration);
            FlameEffect.Stop();
        }

        protected IEnumerator WaitBetweenShootsRoutine()
        {
            canShoot = false;
            yield return new WaitForSeconds(Weapon.DelayBetweenShoots);
            canShoot = true;
        }

        public void Scope()
        {
            if (!WeaponAim) return;
            WeaponAim.SetScope();
        }

        protected void AdjustRecoil()
        {
            var recoilX = Weapon.RecoilX;
            var recoilY = Weapon.RecoilY;
            var recoilZ = Weapon.RecoilZ;

            if (WeaponAim && WeaponAim.IsAiming)
            {
                // Reduce recoil by half when aiming
                recoilX /= 4f;
                recoilY /= 4f;
                recoilZ /= 4f;

                Recoil.ApplyRecoil(recoilX, recoilY, recoilZ);
            }
            else
                Recoil.ApplyRecoil(recoilX, recoilY, recoilZ);
        }

        public void ResetRecoil()
        {
            Recoil.ResetRecoil();
        }
    }
}