using System.Collections.Generic;
using ArmorSystem.Backend;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Web.User;

namespace Player_Controller
{
    public class PlayerNetCode : NetworkBehaviour
    {
        public static PlayerNetCode Singleton { get; private set; }

        private NetworkVariable<ulong> _gettedClientId = new NetworkVariable<ulong>();

        [Header("Attached Components")] [SerializeField]
        private List<Collider> _colliders;
        public NetworkVariable<int> ActiveItemId { get; set; } = new NetworkVariable<int>();

        [Header("In Hand Items")] [SerializeField]
        private InHandObjectsContainer _inHandObjectsContainer;

        [Header("Armor")] public NetworkVariable<int> ActiveArmorId = new(101, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [SerializeField] private ArmorsContainer _armorsContainer;

        [Header("NickName")] [SerializeField] private NetworkVariable<int> _playerId = new(-1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        
        [SerializeField] private List<TMP_Text> _nickNameTexts = new List<TMP_Text>();
  

        private void OnEnable()
            => GlobalEventsContainer.ShouldDisplayHandItem += SendChangeInHandItem;

        private void OnDisable()
            => GlobalEventsContainer.ShouldDisplayHandItem -= SendChangeInHandItem;

        public void EnableColliders(bool value)
        {
            foreach (var collider in _colliders)
                collider.enabled = value;
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Singleton = this;
                _playerId.Value = UserDataHandler.singleton.UserData.Id;
            }

            _gettedClientId.Value = GetClientId();
            AssignName();

            ActiveItemId.OnValueChanged += (int prevValue, int newValue) =>
            {
                if (GetClientId() != _gettedClientId.Value) return;
                _inHandObjectsContainer.DisplayItems(ActiveItemId.Value);
            };

            ActiveArmorId.OnValueChanged += (int prevValue, int newValue) =>
            {
                if (GetClientId() != _gettedClientId.Value) return;
                _armorsContainer.DisplayArmor(ActiveArmorId.Value, this);
            };

            _playerId.OnValueChanged += (int prevValue, int newValue) => { AssignName(); };

            _inHandObjectsContainer.DisplayItems(ActiveItemId.Value);
            _armorsContainer.DisplayArmor(ActiveArmorId.Value, this);
        }

        private void AssignName()
        {
            string name = UserDataHandler.singleton.UserData.Name;
            foreach (var nickNameText in _nickNameTexts)
                nickNameText.text = name;
        }
        
        public ulong GetClientId()
            => OwnerClientId;

        private void SendChangeInHandItem(int itemId, ulong clientId)
        {
            if (!IsOwner) return;
            ChangeInHandItemServerRpc(itemId, clientId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeInHandItemServerRpc(int itemId, ulong clientId)
        {
            ActiveItemId.Value = -1;
            ActiveItemId.Value = itemId;
            _gettedClientId.Value = clientId;
        }
    }
}