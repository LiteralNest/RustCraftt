namespace Lock_System
{
    public interface ILockable
    {
        public void Lock(Locker locker);
        public bool IsLocked();
    }
}