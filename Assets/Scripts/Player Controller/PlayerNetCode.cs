using System.Collections.Generic;
using System.Threading.Tasks;
using CharacterStatsSystem;
using Events;
using Inventory_System;
using Inventory_System.ItemInfo;
using OnPlayerItems;
using PlayerDeathSystem;
using Sound_System;
using Storage_System;
using TMPro;
using UI.Hp_Panel;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace Player_Controller
{
    public class PlayerNetCode : NetworkBehaviour
    {
        public static PlayerNetCode Singleton { get; private set; }
        [field: SerializeField] public ActiveInvetoriesHandler ActiveInvetoriesHandler { get; private set; }
        [field: SerializeField] public ObjectHpDisplayer ObjectHpDisplayer { get; private set; }
        [field: SerializeField] public ResourcesDropper ResourcesDropper { get; private set; }
        [field: SerializeField] public ItemInfoHandler ItemInfoHandler { get; private set; }
        [field: SerializeField] public PlayerSoundsPlayer PlayerSoundsPlayer { get; private set; }
        [field: SerializeField] public InHandObjectsContainer InHandObjectsContainer { get; private set; }
        [field: SerializeField] public CharacterInventory CharacterInventory { get; private set; }
        [field: SerializeField] public PlayerMeleeDamager PlayerMeleeDamager { get; private set; }
        [field: SerializeField] public PlayerKiller PlayerKiller { get; private set; }
        [field: SerializeField] public PlayerKnockDowner PlayerKnockDowner { get; private set; }

        public CharacterStats CharacterStats { get; private set; }


        [Header("In Hand Items")] [SerializeField]
        private InHandObjectsContainer _inHandObjectsContainer;

        [Header("Armor")] public NetworkVariable<int> ActiveArmorId = new(101, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [SerializeField] private ArmorsContainer _armorsContainer;


        [Header("NickName")] [SerializeField] private NetworkVariable<int> _playerId = new(-1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [SerializeField] private List<TMP_Text> _nickNameTexts = new List<TMP_Text>();

        [field: SerializeField] public NetworkVariable<int> ActiveItemId { get; set; } = new NetworkVariable<int>(-1);

        private RigidbodyConstraints _cachedConstraints;

        private void OnEnable()
        {
            GlobalEventsContainer.ShouldDisplayHandItem += SendChangeInHandItem;
            CharacterStatsEventsContainer.OnCharacterStatsAssign += AssignCharacterStats;
        }

        private void OnDisable()
        {
            GlobalEventsContainer.ShouldDisplayHandItem -= SendChangeInHandItem;
            CharacterStatsEventsContainer.OnCharacterStatsAssign -= AssignCharacterStats;
        }
        
        private async void Start()
        {
            await Task.Delay(1000);
            if (IsOwner)
            {
                _playerId.Value = UserDataHandler.Singleton.UserData.Id;
                Singleton = this;
                GlobalEventsContainer.PlayerNetCodeAssigned?.Invoke(this);
            }
        }

        public override void OnNetworkSpawn()
        {
            _inHandObjectsContainer.DisplayItems(ActiveItemId.Value);

            ActiveItemId.OnValueChanged += (int prevValue, int newValue) =>
            {
                _inHandObjectsContainer.DisplayItems(ActiveItemId.Value);
            };

            _inHandObjectsContainer.DisplayItems(ActiveItemId.Value);

            ActiveArmorId.OnValueChanged += (int prevValue, int newValue) =>
            {
                _armorsContainer.DisplayArmor(ActiveArmorId.Value, this);
            };

            _armorsContainer.DisplayArmor(ActiveArmorId.Value, this);

            _playerId.OnValueChanged += (int prevValue, int newValue) => { AssignName(); };

            AssignName();
        }

        private void AssignCharacterStats(CharacterStats characterStats)
        {
            CharacterStats = characterStats;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetDefaultHandsServerRpc()
        {
            ActiveItemId.Value = -1;
        }

        private void AssignName()
        {
            string name = UserDataHandler.Singleton.UserData.Name;
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
        }

        [ClientRpc]
        public void SitClientRpc()
        {
            GetComponent<PlayerJumper>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
        }

        [ClientRpc]
        public void StandClientRpc()
        {
            GetComponent<PlayerJumper>().enabled = true;
            GetComponent<PlayerController>().enabled = true;
        }

        [ClientRpc]
        public void ChangePositionClientRpc(Vector3 position)
        {
            if (!IsOwner) return;
            transform.position = position;
        }
    }
}