using System.Collections;
using UnityEngine;

public class LongRangeWeaponObject : WeaponObject
{
    [Header("Attached Objects")] [SerializeField]
    private Camera _targetCamera;
    [SerializeField] private Transform _ammoSpawnPoint;

    [Header("Main Params")] [SerializeField]
    private LongRangeWeapon _weapon;
    
    [Header("In Game Init")] [SerializeField]
    private int _currentAmmoCount;

    private bool _canShoot;
    
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
        => _canShoot = true;

    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(_weapon.ReloadingTime);
        _currentAmmoCount = _weapon.MagazineAmmoCount;
    }

    private IEnumerator StartDelayBetweenShootsRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_weapon.DelayBetweenShoots);
        _canShoot = true;
    }
    
    [ContextMenu("Shoot")]
    public override void Attack()
    {
        if(!_canShoot || _currentAmmoCount <= 0) return;
        
        Ray ray = _targetCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);
        
        Vector3 directionWithoutSpread = targetPoint - _ammoSpawnPoint.position;
        
        float x = Random.Range(-_weapon.Spread, _weapon.Spread);
        float y = Random.Range(-_weapon.Spread, _weapon.Spread);
        
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        AmmoNetworkPool.singleton.SpawnObjectServerRpc(_weapon.Ammo.AmmoPoolId, _weapon.Id,
            _ammoSpawnPoint.position, _ammoSpawnPoint.rotation,  directionWithoutSpread);
        _currentAmmoCount--;
        if (_currentAmmoCount <= 0)
        {
            StartCoroutine(ReloadRoutine());
            return;
        }
        StartCoroutine(StartDelayBetweenShootsRoutine());
    }
}