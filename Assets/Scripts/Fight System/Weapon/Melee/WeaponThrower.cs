using System;
using UnityEngine;

public class WeaponThrower : MonoBehaviour
{
    [Header("Attached Compontents")] 
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ThrowingWeapon _targetPref;
    
    [Header("Physics")]
    [SerializeField] private float _throwForce = 40f;

    private ThrowingWeapon _target;

    private void OnEnable()
    {
        MainUiHandler.singleton.ActivateAttackButton(false);
    }

    private void OnDisable()
    {
        MainUiHandler.singleton.ActivateMeleeThrowButton(false);
        GlobalEventsContainer.WeaponMeleeObjectAssign?.Invoke(null);
    }
    
    private void SpawnSpear(Transform spawnPoint)
    {
        _target = Instantiate(_targetPref, spawnPoint.position, spawnPoint.rotation);
        //Дописати неткод
    }
    
    public void ThrowSpear()
    {
        SpawnSpear(_spawnPoint);
        // var direction = _spawnPoint.forward * _throwForce;
        _target.Throw( _throwForce);
        gameObject.SetActive(false);
    }
}
