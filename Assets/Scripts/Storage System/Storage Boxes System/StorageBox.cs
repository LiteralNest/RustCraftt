using Lock_System;
using Web.UserData;

namespace Storage_System.Storage_Boxes_System
{
    public class StorageBox : DropableStorage, ILockable
    {
        private Locker _locker;

        public override bool CanInteract()
            => _locker == null || _locker.CanBeOpened(UserDataHandler.Singleton.UserData.Id);

        #region ILockable

        public void Lock(Locker locker)
            => _locker = locker;

        bool ILockable.IsLocked()
            => _locker != null;

        #endregion
    }
}