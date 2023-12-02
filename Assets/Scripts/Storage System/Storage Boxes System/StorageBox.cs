using Lock_System;
using UnityEngine;
using Web.User;

namespace Storage_System.Storage_Boxes_System
{
    public class StorageBox : Storage, ILockable
    {
        [SerializeField] private Transform _lockingParrent;
        private KeyLocker _doorLocker;
        private CodeLocker _codeLocker;
    
        public override void Open(InventoryHandler handler)
        {
            if (_doorLocker != null && !_doorLocker.CanBeOpened(UserDataHandler.singleton.UserData.Id)) return;
            if (_codeLocker != null && !_codeLocker.CanBeOpened(UserDataHandler.singleton.UserData.Id, _codeLocker.Password)) return;
            SlotsDisplayer = handler.LargeStorageSlotsDisplayer;
            handler.InventoryPanelsDisplayer.OpenLargeChestPanel();
            base.Open(handler);
        }

        public override void CheckCells()
        {
        }
    
        #region ILockable

        public void LockByKey(KeyLocker locker)
            => _doorLocker = locker;

        public void LockByCode(CodeLocker locker)
        {
            _codeLocker = locker;
        }

        public Transform GetParent()
            => _lockingParrent;

        public bool IsLocked
            => _doorLocker != null;
        
        #endregion
    }
}