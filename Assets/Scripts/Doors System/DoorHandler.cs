using Lock_System;
using Unity.Netcode;
using UnityEngine;

public class DoorHandler : NetworkBehaviour, ILockable
{
    [SerializeField] private Transform _mainTransform;
    [SerializeField] private Animator _anim;
    private KeyLocker _doorLocker;
    private CodeLocker _codeLocker;
    private static readonly int Opened = Animator.StringToHash("Opened");

    private void Start()
        => gameObject.tag = "Door";

    public void Open(int id)
    {
        if (_doorLocker != null && !_doorLocker.CanBeOpened(id)) return;
        _anim.SetBool(Opened, !_anim.GetBool(Opened));
    }

    #region ILockable

    public void LockByKey(KeyLocker locker)
        => _doorLocker = locker;

    public void LockByCode(CodeLocker locker)
    {
        _codeLocker = locker;
    }

    public Transform GetParent()
        => _mainTransform;

    public bool IsLocked
        => _doorLocker != null;

    #endregion
}