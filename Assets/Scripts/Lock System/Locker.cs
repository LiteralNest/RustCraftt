using Unity.Netcode;
using UnityEngine;

namespace Lock_System
{
   public abstract class Locker : NetworkBehaviour
   {
      [field: SerializeField] public GameObject Model { get; private set; }

      public abstract bool CanBeOpened(int value);
      public virtual bool IsLocked() => true;
      public virtual void Open() {}
      public virtual void Init(int userId) {}
   }
}
