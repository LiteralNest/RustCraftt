using System.Collections;
using UnityEngine;

public class LongRangeWeapon : MonoBehaviour
{
    [Header("Main Params")] 
    [SerializeField] private int _magazineAmmoCount;
    [SerializeField] private float _reloadingTime;
    [SerializeField] private Ammo _ammo;
    [SerializeField] private float _firePower;
    [SerializeField] private Transform _ammoSpawnPoint;
    
    [Header("In Game Init")] [SerializeField]
    private int _currentAmmoCount;
    
    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(_reloadingTime);
        _currentAmmoCount = _magazineAmmoCount;
    }
    
    [ContextMenu("Shoot")]
    public void TryShoot()
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