using Unity.Netcode;

namespace Lock_System
{
   public abstract class Locker : NetworkBehaviour
   {
      public ILockable TargetLockable { get; set; }
      
      public abstract bool CanBeOpened(int value);
      public virtual bool IsLocked() => true;
      public virtual void Open() {}
      public virtual void Init(int userId) {}
   }
}
