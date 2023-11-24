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
            if (other.CompareTag("Lock"))
            {
                var keyLocker = other.GetComponent<KeyLocker>();
                if (keyLocker == null) return;

                var lockable = LocakableObject.GetComponent<ILockable>();
                if (lockable == null || lockable.Locked) return;

                TargetLock = other.transform;
                TargetLock.GetComponent<NetworkObject>().TrySetParent(lockable.GetParent());
                var locker = other.GetComponent<KeyLocker>();
                lockable.LockByKey(locker);
                Destroy(gameObject);
            }
            
            else if (!other.CompareTag("CodeLock"))
            {
                var codeLocker = other.GetComponent<CodeLocker>();
                if (codeLocker == null) return;

                var lockable = LocakableObject.GetComponent<ILockable>();
                if (lockable == null || lockable.Locked) return;

                TargetLock = other.transform;
                TargetLock.GetComponent<NetworkObject>().TrySetParent(lockable.GetParent());
                var locker = other.GetComponent<CodeLocker>();
                lockable.LockByCode(locker);
                Destroy(gameObject);
            }
            
        }
    }
}
