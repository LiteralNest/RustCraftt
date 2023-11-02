using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(WeaponSoundPlayer))]
public class ShotgunObject : WeaponObject
{
    [FormerlySerializedAs("_soudPlayer")]
    [Header("Attached Objects")]
    [SerializeField] private WeaponSoundPlayer _soundPlayer;
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

    [Header("Shotgun Settings")]
    [SerializeField] private int _pelletCount = 12;
    [SerializeField] private float _spreadAngle = 20f;

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
        // _canShoot = false;
        yield return new WaitForSeconds(Weapon.DelayBetweenShoots);
        // _canShoot = true;
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
        var spawnPoint = _ammoSpawnPoint.position;
        var shootDirection = transform.forward;

        for (int i = 0; i < _pelletCount; i++)
        {
            var spreadWorldPos = Random.insideUnitCircle * _spreadAngle;

            var spreadOffset = new Vector3(spreadWorldPos.x, spreadWorldPos.y, 0f);

            var shootRay = new Ray(spawnPoint, shootDirection + spreadOffset);

            RecoilFire();

            if (Physics.Raycast(shootRay, out var hit, Weapon.Range, _targetMask))
            {
                TryDamage(hit);
                DisplayHit(hit);
            }
        }
    }


    public override void Attack(bool value)
    {
        if (!_canShoot || _currentAmmoCount <= 0) return;

        _soundPlayer.PlayShot();
        MinusAmmo();
        SpreadShots(); // Need to add logic of scope
        StartCoroutine(WaitBetweenShootsRoutine());
    }
}