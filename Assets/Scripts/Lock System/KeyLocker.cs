using Unity.Netcode;
using UnityEngine;

public class KeyLocker : NetworkBehaviour
{
    private NetworkVariable<ushort> _registratedKey = new(0, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public void RegistrateKey(int userId)
    {
        _registratedKey.Value = (ushort)userId;
        Debug.Log("Key registrated");
    }

    public bool CanBeOpened(int value)
        => _registratedKey.Value == value;
}