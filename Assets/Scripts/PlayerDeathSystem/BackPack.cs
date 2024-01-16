using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem
{
    public class BackPack : Storage
    {
        [SerializeField] private NetworkObject _networkObject;
        [SerializeField] private Transform _mapPoint;
        [SerializeField] private PlayerCorpDisplay _playerCorpDisplay;

        public PlayerCorpDisplay PlayerCorpDisplay => _playerCorpDisplay;
        public NetworkVariable<bool> WasDisconnected { get; private set; } = new(false);
        public NetworkVariable<int> OwnerId { get; private set; } = new(-1);

        private void OnDestroy()
            => Destroy(_mapPoint.gameObject);
        
        public void SetWasDisconnectedAndOwnerId(bool value, int ownerId)
        {
            if (!IsServer)
            {
                Debug.LogError("Must be server for setting values!");
                return;
            }
            WasDisconnected.Value = value;
            OwnerId.Value = ownerId;
        }

        [ServerRpc(RequireOwnership = false)]
        public void DespawnServerRpc()
        {
            if (!IsServer) return;
            _networkObject.Despawn();
        }
    }
}