using UnityEngine;

namespace Lock_System
{
    public interface ILockable
    {
        public void Lock(KeyLocker locker);
        public Transform GetParent();
        public bool Locked { get; }
    }
}