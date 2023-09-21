using UnityEngine;
using UnityEngine.Serialization;

public abstract class WeaponObject : MonoBehaviour
{
    [SerializeField] private bool _canBeReloaded = false;
    
    public abstract void Attack();
    public virtual async void Reload(){}
    public virtual bool CanReload() => _canBeReloaded;
}
