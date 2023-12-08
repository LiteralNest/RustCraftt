using Unity.Netcode;
using UnityEngine;

namespace Lock_System
{
    public class ObjectLocker : NetworkBehaviour
    {
        public GameObject LocakableObject;
        public Transform TargetLock { get; private set; }

        [SerializeField] private KeyLocker _keyLocker;
        [SerializeField] private CodeLocker _codeLocker;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Lock"))
            {
                var keyLocker = other.GetComponent<KeyLocker>();
                if (keyLocker == null) return;

                var lockable = LocakableObject.GetComponent<ILockable>();
                if (lockable == null || lockable.IsLocked()) return;

                _keyLocker.gameObject.SetActive(true);
                TargetLock = _keyLocker.transform;
                
                lockable.Lock(_keyLocker);
                _keyLocker.TargetLockable = lockable;
                
                Destroy(gameObject);
            }
            
            else if (!other.CompareTag("CodeLocker"))
            {
                var codeLocker = other.GetComponent<CodeLocker>();
                if (codeLocker == null) return;

                var lockable = LocakableObject.GetComponent<ILockable>();
                if (lockable == null || lockable.IsLocked()) return;

                TargetLock = _codeLocker.transform;
                
                lockable.Lock(_codeLocker);
                _codeLocker.TargetLockable = lockable;
                Destroy(gameObject);
            }
        }
    }
}
