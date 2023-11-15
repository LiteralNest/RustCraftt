using Lock_System;
using Storage_Boxes;
using UnityEngine;
using Web.User;

public class ToolClipboard : Storage, ILockable
{
    [SerializeField] private Transform _mainTransform;
    private KeyLocker _doorLocker;
    
    public override void Open(InventoryHandler handler)
    {
        if (_doorLocker != null && !_doorLocker.CanBeOpened(UserDataHandler.singleton.UserData.Id)) return;
        SlotsDisplayer = handler.ToolClipboardSlotsDisplayer;
        handler.OpenClipBoardPanel();
        base.Open(handler);
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