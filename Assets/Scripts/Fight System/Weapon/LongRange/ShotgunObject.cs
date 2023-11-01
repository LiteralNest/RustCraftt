using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class ShotgunObject : WeaponObject
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
    private Camera _cam;

    private LongRangeWeaponInventoryItemDisplayer _inventoryItemDisplayer;

    [Header("Shotgun Settings")]
    [SerializeField] private int _pelletCount = 12;
    [SerializeField] private float _spreadAngle = 20f;

    private void Start()
    {
        _cam = Camera.main;
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
        if (!hit.transform.TryGetComponent<IDamagable>(out var damageableObj)) return;
        damageableObj.GetDamage((int)(Weapon.Damage * Weapon.Ammo.MultiplyKoef));
    }

    public override async void Reload()
    {
        await Task.Delay(1000);
        _currentAmmoCount = Weapon.MagazineCount;
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
        var fire = Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(fire, 2f);
        var decal = Instantiate(_decal, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(decal, 5);
    }

    private void SpreadShots()
    {
        for (int i = 0; i < _pelletCount; i++)
        {
            Vector3 shootDirection = _cam.transform.forward;

            Vector2 spreadWorldPos = Random.insideUnitCircle * _spreadAngle;

            Vector3 spreadOffset = new Vector3(spreadWorldPos.x, spreadWorldPos.y, 0f);

            Ray shootRay = new Ray(_cam.transform.position, shootDirection + spreadOffset);

            RaycastHit hit;

            RecoilFire();

            if (Physics.Raycast(shootRay, out hit, Weapon.Range, _targetMask))
            {
                TryDamage(hit);
                DisplayHit(hit);
            }
        }
    }

    public override void Attack(bool value)
    {
        if (!_canShoot || _currentAmmoCount <= 0) return;

        _soudPlayer.PlayShot();
        MinusAmmo();
        SpreadShots(); // Need to add logic of scope
        StartCoroutine(WaitBetweenShootsRoutine());
    }
}