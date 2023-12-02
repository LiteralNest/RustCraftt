using Lock_System;
using UnityEngine;
using Web.User;

namespace Storage_System.Storage_Boxes_System
{
    public class StorageBox : Storage, ILockable
    {
        [SerializeField] private Transform _lockingParrent;

        private Locker _locker;
        
        public override void Open(InventoryHandler handler)
        {
            if (_locker != null && !_locker.CanBeOpened(UserDataHandler.singleton.UserData.Id)) return;
            SlotsDisplayer = handler.LargeStorageSlotsDisplayer;
            handler.InventoryPanelsDisplayer.OpenLargeChestPanel();
            base.Open(handler);
        }

        public override void CheckCells()
        {
        }
    
        #region ILockable

        public void Lock(Locker locker)
            => _locker = locker;

        public Transform GetParent()
            => _lockingParrent;

        bool ILockable.IsLocked()
            => _locker != null;

        #endregion
    }
}