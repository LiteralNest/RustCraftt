using Storage_System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem
{
    public class BackPack : Storage
    {
        [SerializeField] private NetworkObject _networkObject;
        [SerializeField] private Transform _mapPoint;
        [SerializeField] private PlayerCorpDisplay _playerCorpDisplay;
        [SerializeField] private TMP_Text _nickNameText;

        public PlayerCorpDisplay PlayerCorpDisplay => _playerCorpDisplay;
        public NetworkVariable<bool> WasDisconnected { get; private set; } = new(false);
        public NetworkVariable<int> OwnerId { get; private set; } = new(-1);
        public NetworkVariable<FixedString64Bytes> NickName { get; set; } = new();

        private void OnDestroy()
            => Destroy(_mapPoint.gameObject);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            RedisplayCloth();
            ItemsNetData.OnValueChanged += (oldValue, newValue) => RedisplayCloth();
            _nickNameText.text = NickName.Value.ToString();
            NickName.OnValueChanged += (FixedString64Bytes oldValue, FixedString64Bytes newValue) =>
                _nickNameText.text = newValue.ToString();
        }
        
        
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

        private void RedisplayCloth()
        {
            _playerCorpDisplay.DisplayCloth(GetArmorSlots());
        }
    }
}