using System;
using System.Collections;
using UnityEngine;

public class LongRangeWeapon : WeaponObject
{
    [Header("Main Params")] 
    [SerializeField] private int _magazineAmmoCount;
    [SerializeField] private float _reloadingTime;
    [SerializeField] private Ammo _ammo;
    [SerializeField] private float _firePower;
    [SerializeField] private Transform _ammoSpawnPoint;
    
    [Header("In Game Init")] [SerializeField]
    private int _currentAmmoCount;

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

    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(_reloadingTime);
        _currentAmmoCount = _magazineAmmoCount;
    }
    
    [ContextMenu("Shoot")]
    public override void Attack()
    {
        if (_currentAmmoCount == 0) return;
        var instance = Instantiate(_ammo, _ammoSpawnPoint.position, new Quaternion(0, 0, 0, 0), _ammoSpawnPoint);
        instance.transform.eulerAngles = new Vector3(0, -90, 0);
        var force = _ammoSpawnPoint.TransformDirection(Vector3.forward * _firePower);
        instance.Fly(force);
        _currentAmmoCount--;
        if (_currentAmmoCount > 0) return;
        StartCoroutine(ReloadRoutine());
    }
}