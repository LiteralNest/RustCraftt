using Lock_System;
using UnityEngine;
using Web.User;

public class StorageBox : Storage, ILockable
{
    [SerializeField] private Transform _lockingParrent;
    private KeyLocker _doorLocker;

    public override void InitBox()
    {
        SaveNetData();
    }

    public override void Open(InventoryHandler handler)
        => OpenBox(handler);

    public override void CheckCells()
    {
    }

    private void OpenBox(InventoryHandler handler)
    {
        if (_doorLocker != null && !_doorLocker.CanBeOpened(UserDataHandler.singleton.UserData.Id)) return;
        handler.LargeStorageSlotsContainer.InitCells(Cells, this);
        handler.OpenLargeChestPanel();
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