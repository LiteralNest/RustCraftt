using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace Lock_System
{
    public class ObjectLocker : NetworkBehaviour
    {
        [SerializeField] private KeyLocker _keyLocker;
        [SerializeField] private CodeLocker _codeLocker;
        [SerializeField] private MonoBehaviour _lockable;
        [SerializeField] private NetworkVariable<int> _activeLockId = new(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _activeLockId.OnValueChanged += (oldValue, newValue) =>
            {
                DisplayCurrentLock(newValue);
            };
            DisplayCurrentLock(_activeLockId.Value);
        }

        private void DisplayCurrentLock(int id)
        {
            var lockable = _lockable.GetComponent<ILockable>();
            if (id == 1)
            {
                lockable.Lock(_keyLocker);
                _keyLocker.Model.SetActive(true);
                _codeLocker.Model.SetActive(false);
            }
            else if (id == 2)
            {
                lockable.Lock(_codeLocker);
                _keyLocker.Model.SetActive(false);
                _codeLocker.Model.SetActive(true);
            }
            else
            {
                _keyLocker.Model.SetActive(false);
                _codeLocker.Model.SetActive(false);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void LockServerRpc(int userId, int lockId)
        {
            if (!IsServer) return;
            _activeLockId.Value = lockId;
            if (lockId == 1)
                _keyLocker.Init(userId);
            else if (lockId == 2)
                _codeLocker.Init(userId);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Lock"))
            {
                var lockable = _lockable.GetComponent<ILockable>();
                if (lockable == null || lockable.IsLocked()) return;
                
                Destroy(other.gameObject);
                LockServerRpc(UserDataHandler.Singleton.UserData.Id, 1);

                lockable.Lock(_keyLocker);
                gameObject.tag = "Untagged";
            }
 
            else if (other.CompareTag("CodeLocker"))
            {
                var lockable = _lockable.GetComponent<ILockable>();
                if (lockable == null || lockable.IsLocked()) return;
                
                Destroy(other.gameObject);
                LockServerRpc(UserDataHandler.Singleton.UserData.Id, 2);

                lockable.Lock(_codeLocker);
                gameObject.tag = "Untagged";
            }
        }
    }
}