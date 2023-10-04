using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(WeaponSoudPlayer))]
public class LongRangeWeaponObject : WeaponObject
{
    [Header("Attached Objects")]
    [SerializeField] private WeaponSoudPlayer _soudPlayer;
    [SerializeField] private Transform _ammoSpawnPoint;
    [SerializeField] private GameObject _impactEffect;
    [SerializeField] private GameObject _flameEffect;
    [SerializeField] private float _flameEffectDuration;
    
    [field:SerializeField] public LongRangeWeapon Weapon { get; private set; }
    [SerializeField] private GameObject _decal;
    [SerializeField] private LayerMask _targetMask;
    
    [Header("In Game Init")] 
    private int _currentAmmoCount;

    private bool _canShoot;
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;

    private LongRangeWeaponInventoryItemDisplayer _inventoryItemDisplayer;
    
    private void OnEnable()
    {
        GlobalEventsContainer.AttackButtonActivated?.Invoke(true);
        GlobalEventsContainer.WeaponObjectAssign?.Invoke(this);
    }

    private void OnDisable()
    {
        GlobalEventsContainer.AttackButtonActivated?.Invoke(false);
        GlobalEventsContainer.WeaponObjectAssign?.Invoke(null);
    }

    private void Start()
    {
        Reload();
        _canShoot = true;
    }

    private void Update()
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, Weapon.ReturnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, Weapon.Snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }
    
    private void RecoilFire()
    {
        _targetRotation += new Vector3(Weapon.RecoilX, Random.Range(-Weapon.RecoilY, Weapon.RecoilY),
            Random.Range(-Weapon.RecoilZ, Weapon.RecoilZ));
    }
    
    private IEnumerator WaitBetweenShootsRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(Weapon.DelayBetweenShoots);
        _canShoot = true;
    }

    private void TryDamage(RaycastHit hit)
    {
        if (!hit.transform.TryGetComponent<IDamagable>(out var damagableObj)) return;
        damagableObj.GetDamage((int)(Weapon.Damage * Weapon.Ammo.MultiplyKoef));
    }
    
    // private int GetInventoryAmmoCount() 
    //     => InventorySlotsContainer.singleton.GetItemCount(Weapon.Ammo);
    
    // public override bool CanReload()
    //     => GetInventoryAmmoCount() > 0;

    public override async void Reload()
    {
        // int count = GetInventoryAmmoCount();
        // if (count > Weapon.MagazineCount)
        //     count = Weapon.MagazineCount;
        // count -= _currentAmmoCount;
        // InventorySlotsContainer.singleton.DeleteSlot(Weapon.Ammo, count);
        await Task.Delay(1000);
        // _currentAmmoCount = count; //Дописати логіку перевірки в інвентарі
    }
    
    private void MinusAmmo()
    {
        GlobalEventsContainer.ShouldDisplayReloadingButton?.Invoke(true);
        _currentAmmoCount--;
        if(_currentAmmoCount <= 0)
            Reload();
    }

    private async void DisplayFlameEffect()
    {
        _flameEffect.SetActive(true);
        await Task.Delay((int)(_flameEffectDuration * 1000));
        _flameEffect.SetActive(false);
    }

    private void DisplayHit(RaycastHit hit)
    {
        var fire = Instantiate(_impactEffect, hit.point,
            Quaternion.LookRotation(hit.normal));
        Destroy(fire, 2f);
        var decal = Instantiate(_decal, hit.point,
            Quaternion.LookRotation(hit.normal));
        Destroy(decal, 5);
    }
    
    public override void Attack()
    {
        if (!_canShoot || _currentAmmoCount <= 0) return;
        RaycastHit hit;
        _soudPlayer.PlayShot();
        MinusAmmo();
        RecoilFire();
        DisplayFlameEffect();
        if (Physics.Raycast(transform.position, transform.forward, out hit,
                Weapon.Range, _targetMask))
        {
            TryDamage(hit);
            DisplayHit(hit);
        }
        StartCoroutine(WaitBetweenShootsRoutine());
    }
}