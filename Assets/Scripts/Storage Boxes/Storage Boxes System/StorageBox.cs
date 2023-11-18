using Lock_System;
using Storage_Boxes;
using UnityEngine;
using Web.User;

public class StorageBox : Storage, ILockable
{
    [SerializeField] private Transform _lockingParrent;
    private KeyLocker _doorLocker;
    
    public override void Open(InventoryHandler handler)
    {
        if (_doorLocker != null && !_doorLocker.CanBeOpened(UserDataHandler.singleton.UserData.Id)) return;
        SlotsDisplayer = handler.LargeStorageSlotsDisplayer;
        handler.InventoryPanelsDisplayer.OpenLargeChestPanel();
        base.Open(handler);
    }

    public override void CheckCells()
    {
    }
    
    #region ILockable

    public void Lock(KeyLocker locker)
        => _doorLocker = locker;

    public Transform GetParent()
        => _lockingParrent;

    public bool Locked
        => _doorLocker != null;

    #endregion
}