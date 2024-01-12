using System.Collections;
using Events;
using FightSystem.Damage;
using FightSystem.Weapon.ShootWeapon.Sway;
using FightSystem.Weapon.ShootWeapon.TrailSystem;
using FightSystem.Weapon.WeaponViewSystem;
using Items_System.Items.Weapon;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;

namespace FightSystem.Weapon.ShootWeapon
{
    [RequireComponent(typeof(TrailSpawner))]
    [RequireComponent(typeof(WeaponSway))]
    [RequireComponent(typeof(AimSway))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class BaseShootingWeapon : NetworkBehaviour
    {
        private const string ViewName = "Weapon/View/LongRangeWeaponView";

        [Header("Main Parameters")] [SerializeField]
        private bool _isSingle;

        [SerializeField] protected LayerMask TargetMask;
        [SerializeField] protected int _bulletSpeed = 100;

        [Header("Attached Components")] 
        [SerializeField] protected GameObject Decal;
        [SerializeField] private WeaponAnimator _weaponAnimator;
        [SerializeField] private Transform _swayTransform;
        [SerializeField] protected ShootingWeapon Weapon;
        [SerializeField] protected Transform AmmoSpawnPoint;
        [SerializeField] protected GameObject ImpactEffect;

        [Header("Aim")] 
        [SerializeField] private Transform _aimPosition;
        [SerializeField] private Transform _aimingTransform;

        [Header("Animation")]
        [SerializeField] private AnimationClip _reloadAnim;

        [Header("Flame")] [SerializeField] protected VisualEffect FlameEffect;
        [SerializeField] protected float FlameEffectDuration;

        private WeaponSway _weaponSway;
        private AimSway _aimSway;

        protected WeaponSoundPlayer SoundPlayer;
        private WeaponRecoil _recoil;
        protected WeaponAim WeaponAim;
        private LongRangeWeaponView _weaponView;

        protected TrailSpawner _trailSpawner;

        public bool IsSingle => _isSingle;
        protected int CurrentAmmoCount;
        private bool _canShoot;
        private bool _isShooting;
        private readonly float _timeBetweenShots = 0f;
        private bool _isReloading = false;

        private void OnEnable()
        {
            _isReloading = false;
            if (WeaponAim != null)
                WeaponAim.UnScope();
            TryDisplayReload();
            TryDisplayAttack();
            GlobalEventsContainer.WeaponObjectAssign?.Invoke(this);
            GlobalEventsContainer.InventoryDataChanged += TryDisplayReload;
        }

        private void OnDisable()
        {
            GlobalEventsContainer.InventoryDataChanged -= TryDisplayReload;
            GlobalEventsContainer.WeaponObjectAssign?.Invoke(null);
        }

        private void Start()
        {
            _weaponSway = GetComponent<WeaponSway>();
            _weaponSway.Init(_swayTransform);
            _aimSway = GetComponent<AimSway>();
            _aimSway.Init(_swayTransform);
            SoundPlayer = new WeaponSoundPlayer(GetComponent<AudioSource>(), Weapon);
            WeaponAim = new WeaponAim(_aimPosition, _aimingTransform, _weaponSway, _aimSway);
            _recoil = new WeaponRecoil(_swayTransform);
            _trailSpawner = GetComponent<TrailSpawner>();
            _canShoot = true;
            _weaponView = Instantiate(Resources.Load<LongRangeWeaponView>(ViewName), transform);
            _weaponView.Init(this);
        }

        private void Update()
        {
            if (_isShooting && CanShoot())
                Attack();
            _recoil.UpdateRecoil(3f);
            WeaponAim.UpdateSway();
        }

        protected virtual void Attack()
        {
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

        public void Init()
        {
            CurrentAmmoCount = InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.GetCurrentAmmo();
            if (CurrentAmmoCount > 0) _weaponView.AssignAttack(true);
        }

        public void HandleShoot(bool value)
        {
            if(!value)
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
            _weaponView.AssignReload(false);
            yield return new WaitForSeconds(_reloadAnim.length);
            if(wasUnScopped)
                WeaponAim.SetScope();
            CurrentAmmoCount += count;
            InventoryHandler.singleton.CharacterInventory.RemoveItem(Weapon.Ammo.Id, count);
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.SetCurrentAmmo(CurrentAmmoCount);
            _weaponView.AssignAttack(true);
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

        public void TryDisplayReload()
        {
            if (InventoryHandler.singleton == null || InventoryHandler.singleton.ActiveSlotDisplayer == null ||
                _weaponView == null || CurrentAmmoCount >= Weapon.MagazineCount)
                return;
            var addingAmmo = InventoryHandler.singleton.CharacterInventory.GetItemCount(Weapon.Ammo.Id);
            _weaponView.AssignReload(addingAmmo > 0);
        }

        private void TryDisplayAttack()
        {
            if (_weaponView == null) return;
            _weaponView.AssignAttack(CurrentAmmoCount > 0);
        }

        protected void MinusAmmo()
        {
            CurrentAmmoCount--;
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.MinusCurrentAmmo(1);
            TryDisplayReload();
            TryDisplayAttack();
        }

        protected IEnumerator DisplayFlameEffect()
        {
            FlameEffect.Play();
            yield return new WaitForSeconds(FlameEffectDuration);
            FlameEffect.Stop();
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