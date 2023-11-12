using UnityEngine;

public class PlayerSpearThrowing : MonoBehaviour
{
    private MeleeShootingWeapon _meleeShootingWeapon;
    private bool _attacking;

    private void OnEnable()
        => GlobalEventsContainer.WeaponMeleeObjectAssign += AssignMeleeWeaponObject;
    
    private void OnDisable()
        => GlobalEventsContainer.WeaponMeleeObjectAssign -= AssignMeleeWeaponObject;
    
    private void Update()
    {
        if (!_attacking || _meleeShootingWeapon == null) return;
        _meleeShootingWeapon.ThrowSpearByPhysic();
            
    }

    private void AssignMeleeWeaponObject(MeleeShootingWeapon value)
    {
        _meleeShootingWeapon = value;
    }
    public void SetAttacking(bool value)
    {
        _attacking = value;
    }

    public void SetOnThrowPosition()
    {
        _meleeShootingWeapon.SetThrowingPosition();
    }
    
    
}