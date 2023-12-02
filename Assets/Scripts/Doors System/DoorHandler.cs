using Lock_System;
using Unity.Netcode;
using UnityEngine;

public class DoorHandler : NetworkBehaviour, ILockable
{
    [SerializeField] private Transform _mainTransform;
    [SerializeField] private Animator _anim;
    private Locker _locker;
    private static readonly int Opened = Animator.StringToHash("Opened");

    private void Start()
        => gameObject.tag = "Door";

    public void Open(int id)
    {
        if (_locker != null && !_locker.CanBeOpened(id)) return;
        _anim.SetBool(Opened, !_anim.GetBool(Opened));
    }

    #region ILockable

    public void Lock(Locker locker)
        => _locker = locker;

    public Transform GetParent()
        => _mainTransform;

    bool ILockable.IsLocked()
        => _locker != null;

    #endregion
}