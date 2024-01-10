using Lock_System;
using Unity.Netcode;
using UnityEngine;

public class DoorHandler : NetworkBehaviour, ILockable
{
    [SerializeField] private Animator _anim;
    private Locker _locker;

    private NetworkVariable<bool> _wasOpened = new();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _wasOpened.OnValueChanged += (bool prevValue, bool newValue) =>
        {
            Debug.Log(newValue);
            OpenClientRpc(newValue);
        };
    }

    private void Start()
        => gameObject.tag = "Door";

    public void Open(int id)
    {
        if (_locker != null && !_locker.CanBeOpened(id)) return;
        OpenServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void OpenServerRpc()
    {
        if (!IsServer) return;
        _wasOpened.Value = !_wasOpened.Value;
    }

    [ClientRpc]
    private void OpenClientRpc(bool value)
    {
        if (value)
            _anim.SetTrigger("Open");
        else
            _anim.SetTrigger("Close");
    }

    #region ILockable

    public void Lock(Locker locker)
        => _locker = locker;


    bool ILockable.IsLocked()
        => _locker != null;

    #endregion
}