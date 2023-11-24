using UnityEngine;

namespace Lock_System
{
    public interface ILockable
    {
        public void LockByKey(KeyLocker locker);
        public void LockByCode(CodeLocker locker);
        public Transform GetParent();
        public bool Locked { get; }
    }
}