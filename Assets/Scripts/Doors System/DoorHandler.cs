using Lock_System;
using Unity.Netcode;
using UnityEngine;

public class DoorHandler : NetworkBehaviour, ILockable
{
    [SerializeField] private Animator _anim;
    private Locker _locker;

    private NetworkVariable<bool> _wasOpened = new();

    private void Start()
        => gameObject.tag = "Door";

    [ServerRpc(RequireOwnership = false)]
    private void SetWasOpenedServerRpc(bool value)
    {
        if (!IsServer) return;
        _wasOpened.Value = value;
    }
    
    public void Open(int id)
    {
        if (_locker != null && !_locker.CanBeOpened(id)) return;
        if(!_wasOpened.Value)
            _anim.SetTrigger("Open");
        else
            _anim.SetTrigger("Close");
        SetWasOpenedServerRpc(!_wasOpened.Value);
    }

    #region ILockable

    public void Lock(Locker locker)
        => _locker = locker;
    

    bool ILockable.IsLocked()
        => _locker != null;

    #endregion
}