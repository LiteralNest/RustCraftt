using UnityEngine;

public class PlayerFightHandler : MonoBehaviour
{
    [SerializeField] private Animator _handsAnimator;
    private WeaponObject _currentWeaponObject;
    private bool _attacking;

    private void OnEnable()
        => GlobalEventsContainer.WeaponObjectAssign += AssignWeaponObject;
    
    private void OnDisable()
        => GlobalEventsContainer.WeaponObjectAssign -= AssignWeaponObject;
    
    private void Update()
    {
        if(!_attacking || _currentWeaponObject == null) return;
    }

    private void AssignWeaponObject(WeaponObject value)
    {
        _currentWeaponObject = value;
        
    }

    public void SetAttacking(bool value)
    {
        _currentWeaponObject.Attack(value);
        _attacking = value;
    }

    public void Reload()
    {
        _currentWeaponObject.Reload();
        GlobalEventsContainer.ShouldDisplayReloadingButton?.Invoke(false);
    }
}
