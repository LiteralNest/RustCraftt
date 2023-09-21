using UnityEngine;

public class PlayerFightHandler : MonoBehaviour
{
    [Header("UI")] [SerializeField] private GameObject _reloadingButton;
    
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
    {
        _currentWeaponObject = value;
        if (value == null)
        {
            _reloadingButton.SetActive(false);
            return;
        }
        if(_currentWeaponObject.CanReload()) _reloadingButton.SetActive(true);
        else _reloadingButton.SetActive(false);
    }

    public void SetAttacking(bool value)
        => _attacking = value;

    public void Reload()
    {
        _currentWeaponObject.Reload();
    }
}
