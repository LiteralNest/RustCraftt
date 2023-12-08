using System.Collections;
using Items_System.Items.Weapon;
using UnityEngine;
using UnityEngine.VFX;

namespace Fight_System.Weapon.ShootWeapon
{
    public abstract class BaseShootingWeapon : MonoBehaviour, IWeapon
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

        public bool IsSingle => _isSingle;
        protected int currentAmmoCount;
        public int CurrentAmmoCount => currentAmmoCount;
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
            return canShoot && _timeBetweenShots <= 0 && currentAmmoCount > 0 && !_isReloading;
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
            currentAmmoCount = count;
            InventoryHandler.singleton.CharacterInventory.RemoveItem(Weapon.Ammo.Id, count);
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.SetCurrentAmmo(currentAmmoCount);
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
            currentAmmoCount--;
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.MinusCurrentAmmo(1);
            if (currentAmmoCount <= 0)
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