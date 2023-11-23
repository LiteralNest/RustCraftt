using Unity.Netcode;
using UnityEngine;

namespace Lock_System
{
    public class CodeLocker : NetworkBehaviour
    {
        private NetworkVariable<ushort> _registratedKey = new(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public void RegistrateCode(int userId)
        {
            _registratedKey.Value = (ushort)userId;
            Debug.Log("Code registrated");
        }

        public bool CanBeOpened(int value)
            => _registratedKey.Value == value;
    }
}