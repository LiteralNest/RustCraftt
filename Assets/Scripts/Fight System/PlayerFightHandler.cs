using Fight_System.Weapon.ShootWeapon;
using UI;
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
        if (_currentBaseShootingWeapon == null) return;
        _currentBaseShootingWeapon.Init();
    }

    public void SetAttacking(bool value)
    {
        if(!_currentBaseShootingWeapon) return;
        _attacking = value;
        if (value == false)
        {
            _currentBaseShootingWeapon.ResetRecoil();
        }
    }

    public void Reload()
    {
        if(!_currentBaseShootingWeapon) return;
        _currentBaseShootingWeapon.Reload();
        CharacterUIHandler.singleton.ActivateReloadingButton(false);
    }

    public void Scope()
    {
        if(!_currentBaseShootingWeapon) return;
        _currentBaseShootingWeapon.Scope();
    }
}