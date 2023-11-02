using Unity.Netcode;
using UnityEngine;

public class Lock : NetworkBehaviour
{
   [SerializeField] private NetworkVariable<ushort> _playerId = new NetworkVariable<ushort>();
}
