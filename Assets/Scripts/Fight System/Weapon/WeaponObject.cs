using UnityEngine;
using UnityEngine.Serialization;

public abstract class WeaponObject : MonoBehaviour
{
    [SerializeField] private bool _canBeReloaded = false;
    [field:SerializeField] public WeaponType WeaponType { get; private set; }
    public abstract void Attack();
    public virtual async void Reload(){}
    public virtual bool CanReload() => _canBeReloaded;
}
