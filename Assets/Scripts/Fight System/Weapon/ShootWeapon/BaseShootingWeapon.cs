using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Fight_System.Weapon.ShootWeapon
{
    public abstract class BaseShootingWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] protected WeaponAim WeaponAim;
        [SerializeField] private bool canBeReloaded = false; //to show in editor for debug

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
        protected bool canShoot;
        private float _timeBetweenShots = 0f; // Variable to handle shots between shoots
        private bool _isReloading = false;

        protected void Start()
            => canShoot = true;

        protected bool CanShoot()
        {
            return canShoot && _timeBetweenShots <= 0 && currentAmmoCount > 0 && !_isReloading;
        }

        private void OnEnable()
        {
            GlobalEventsContainer.ShouldHandleScope?.Invoke(true);
            GlobalEventsContainer.WeaponObjectAssign?.Invoke(this);
        }

        private void OnDisable()
        {
            GlobalEventsContainer.ShouldHandleScope?.Invoke(false);
            GlobalEventsContainer.ShouldDisplayReloadingButton?.Invoke(false);
            GlobalEventsContainer.AttackButtonActivated?.Invoke(false);
            GlobalEventsContainer.WeaponObjectAssign?.Invoke(null);
        }

        public virtual void Attack()
        {
        }

        public virtual bool CanReload() => canBeReloaded;


        public virtual void Reload()
        {
            if (_isReloading) return; // Don't initiate another reload while already reloading.
            _isReloading = true;
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(1f); // Simulate 1 second reload time
            currentAmmoCount = Weapon.MagazineCount;
            _isReloading = false;
        }

        protected void TryDamage(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent<IDamagable>(out var damagableObj))
            {
                damagableObj.GetDamage((int)(Weapon.Damage * Weapon.Ammo.MultiplyKoef));
            }
        }

        protected void DisplayHit(RaycastHit hit)
        {
            var fire = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(fire, 2f);
            var decalObj = Instantiate(Decal, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(decalObj, 5);
        }

        protected void MinusAmmo()
        {
            GlobalEventsContainer.ShouldDisplayReloadingButton?.Invoke(true);
            currentAmmoCount--;
            if (currentAmmoCount <= 0)
            {
                Reload();
            }
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
            if(!WeaponAim) return;
            WeaponAim.SetScope();
        }

        public void ResetRecoil()
        {
            Recoil.ResetRecoil();
        }
    }
}