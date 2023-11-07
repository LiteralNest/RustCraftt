using Lock_System;
using Unity.Netcode;
using UnityEngine;

public class DoorHandler : NetworkBehaviour, ILockable
{
    [SerializeField] private Transform _mainTransform;
    [SerializeField] private Animator _anim;
    private KeyLocker _doorLocker;
    private static readonly int Opened = Animator.StringToHash("Opened");

    private void Start()
        => gameObject.tag = "Door";

    public void Open(int id)
    {
        if (_doorLocker != null && !_doorLocker.CanBeOpened(id)) return;
        _anim.SetBool(Opened, !_anim.GetBool(Opened));
    }

    #region ILockable

    public void Lock(KeyLocker locker)
        => _doorLocker = locker;

    public Transform GetParent()
        => _mainTransform;

    public bool Locked
        => _doorLocker != null;

    #endregion
}