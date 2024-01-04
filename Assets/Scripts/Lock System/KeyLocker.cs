using Unity.Netcode;

namespace Lock_System
{
    public class KeyLocker : Locker
    {
        private NetworkVariable<int> _registratedKey = new(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public override void Init(int userId)
            => _registratedKey.Value = userId;

        public override bool CanBeOpened(int value)
            => _registratedKey.Value == value;
    }
}