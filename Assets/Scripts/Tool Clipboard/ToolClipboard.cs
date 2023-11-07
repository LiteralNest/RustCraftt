using Lock_System;
using UnityEngine;
using Web.User;

public class ToolClipboard : Storage, ILockable
{
    [SerializeField] private Transform _mainTransform;
    private KeyLocker _doorLocker;
    
    public override void Open(InventoryHandler handler)
    {
        if (_doorLocker != null && !_doorLocker.CanBeOpened(UserDataHandler.singleton.UserData.Id)) return;
        handler.ToolClipboardSlotsContainer.InitCells(Cells, this);
        handler.OpenClipBoardPanel();
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