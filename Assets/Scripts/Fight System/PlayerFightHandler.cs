using Fight_System.Weapon.ShootWeapon;
using UnityEngine;

public class PlayerFightHandler : MonoBehaviour
{
    private BaseShootingWeapon _currentBaseShootingWeapon;
    private bool _attacking;

    private void OnEnable()
        => GlobalEventsContainer.WeaponObjectAssign += AssignWeaponObject;
    
    private void OnDisable()
        => GlobalEventsContainer.WeaponObjectAssign -= AssignWeaponObject;
    
    private void Update()
    {
        if(!_attacking || _currentBaseShootingWeapon == null) return;

        if (_currentBaseShootingWeapon.IsSingle)
        {
            _currentBaseShootingWeapon.Attack();
            SetAttacking(false);
        }
        else
        {
            _currentBaseShootingWeapon.Attack();
        }
    }

    private void AssignWeaponObject(BaseShootingWeapon value)
    {
        _currentBaseShootingWeapon = value;
        
    }

    public void SetAttacking(bool value)
    {
        _attacking = value;
    }

    public void Reload()
    {
        _currentBaseShootingWeapon.Reload();
        GlobalEventsContainer.ShouldDisplayReloadingButton?.Invoke(false);
    }

    public void Scope()
    {
        _currentBaseShootingWeapon.Scope();
    }
}