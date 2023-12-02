using UnityEngine;

namespace Lock_System
{
    public interface ILockable
    {
        public void Lock(Locker locker);
        public Transform GetParent();
        public bool IsLocked();
    }
}