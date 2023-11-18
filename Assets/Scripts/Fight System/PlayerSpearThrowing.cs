using UnityEngine;

public class PlayerSpearThrowing : MonoBehaviour
{
    private MeleeShootingWeapon _meleeShootingWeapon;

    private void OnEnable()
        => GlobalEventsContainer.WeaponMeleeObjectAssign += AssignMeleeWeaponObject;

    private void OnDisable()
        => GlobalEventsContainer.WeaponMeleeObjectAssign -= AssignMeleeWeaponObject;

    private void AssignMeleeWeaponObject(MeleeShootingWeapon value)
        => _meleeShootingWeapon = value;

    public void SetAttacking(bool value)
    {
        if(!_meleeShootingWeapon) return;
        _meleeShootingWeapon.Attack(value);
    }

    public void Scope(bool value)
    {
        if(!_meleeShootingWeapon) return;
        _meleeShootingWeapon.SetThrowingPosition(value);
    }
}