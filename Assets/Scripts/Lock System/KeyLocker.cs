using Unity.Netcode;

namespace Lock_System
{
    public class KeyLocker : Locker
    {
        private NetworkVariable<ushort> _registratedKey = new(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public override void Init(int userId)
        => _registratedKey.Value = (ushort)userId;

        public override bool CanBeOpened(int value)
            => _registratedKey.Value == value;
    }
}