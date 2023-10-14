using UnityEngine;

public abstract class WeaponObject : MonoBehaviour
{
    [SerializeField] private bool _canBeReloaded = false;
    [field:SerializeField] public WeaponType WeaponType { get; private set; }
    
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

    
    public abstract void Attack(bool value);
    public virtual async void Reload(){}
    public virtual bool CanReload() => _canBeReloaded;
}
