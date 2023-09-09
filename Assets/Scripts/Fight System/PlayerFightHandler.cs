using UnityEngine;

public class PlayerFightHandler : MonoBehaviour
{
    private WeaponObject _currentWeaponObject;
    private bool _attacking;

    private void OnEnable()
        => GlobalEventsContainer.WeaponObjectAssign += AssignWeaponObject;
    
    private void OnDisable()
        => GlobalEventsContainer.WeaponObjectAssign -= AssignWeaponObject;
    
    private void Update()
    {
        if(!_attacking || _currentWeaponObject == null) return;
        _currentWeaponObject.Attack();
    }
    
    private void AssignWeaponObject(WeaponObject value)
    => _currentWeaponObject = value;
    
    public void SetAttacking(bool value)
        => _attacking = value;
}
