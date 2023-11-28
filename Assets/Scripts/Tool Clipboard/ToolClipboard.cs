using Building_System.Buildings_Connecting;
using Lock_System;
using Storage_System;
using UnityEngine;
using Web.User;

public class ToolClipboard : Storage, ILockable
{
    [SerializeField] private Transform _mainTransform;
    public ConnectedStructure Structure { get; set; }
    private KeyLocker _doorLocker;
    private CodeLocker _codeLocker;

    public override void Open(InventoryHandler handler)
    {
        if (_doorLocker != null && !_doorLocker.CanBeOpened(UserDataHandler.singleton.UserData.Id)) return;
        SlotsDisplayer = handler.ToolClipboardSlotsDisplayer;
        handler.InventoryPanelsDisplayer.OpenClipBoardPanel();
        base.Open(handler);
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

    public bool Locked
        => _doorLocker != null;

    #endregion
}