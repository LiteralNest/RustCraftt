using Unity.Netcode;
using UnityEngine;

namespace Lock_System
{
   public class Lock : NetworkBehaviour
   {
      [SerializeField] private NetworkVariable<ushort> _playerId = new NetworkVariable<ushort>();
   }
}
