using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lock_System
{
    public class ObjectLocker : MonoBehaviour
    {
        public GameObject LocakableObject;
        public Transform TargetLock { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Lock")) return;

            var keyLocker = other.GetComponent<KeyLocker>();
            if(keyLocker == null) return;
            
            var lockable = LocakableObject.GetComponent<ILockable>();
            if(lockable == null || lockable.Locked) return;

            TargetLock = other.transform;
            TargetLock.GetComponent<NetworkObject>().TrySetParent(lockable.GetParent());
            var locker = other.GetComponent<KeyLocker>();
            lockable.Lock(locker);
            Destroy(gameObject);
        }
    }
}
