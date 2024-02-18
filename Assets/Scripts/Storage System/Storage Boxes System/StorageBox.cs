using Cloud.DataBaseSystem.UserData;
using Lock_System;

namespace Storage_System.Storage_Boxes_System
{
    public class StorageBox : DropableStorage, ILockable
    {
        private Locker _locker;

        public override bool CanInteract()
            => _locker == null || _locker.AvailableForOpen(UserDataHandler.Singleton.UserData.Id);

        public override string GetDisplayText()
        {
            if (!CanInteract()) return "Locked";
            return base.GetDisplayText();
        }
        
        #region ILockable

        public void Lock(Locker locker)
            => _locker = locker;

        bool ILockable.IsLocked()
            => _locker != null;

        #endregion
    }
}