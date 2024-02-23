using AlertsSystem;
using Cloud.DataBaseSystem.UserData;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace CharacterStatsSystem
{
    public class CharacterStats : NetworkBehaviour
    {
        [SerializeField] private NetworkVariable<int> _hp = new NetworkVariable<int>(100);
        [SerializeField] private NetworkVariable<int> _food = new NetworkVariable<int>(100);
        [SerializeField] private NetworkVariable<int> _water = new NetworkVariable<int>(100);
        [SerializeField] private NetworkVariable<int> _oxygen = new NetworkVariable<int>(100);

        public NetworkVariable<int> Hp => _hp;
        public NetworkVariable<int> Food => _food;
        public NetworkVariable<int> Water => _water;
        public NetworkVariable<int> Oxygen => _oxygen;


        private void OnEnable()
            => CharacterStatsEventsContainer.OnCharacterStatsAssign += InitStatsValue;

        private void OnDisable()
        {
            CharacterStatsEventsContainer.OnCharacterStatsAssign -= InitStatsValue;
            CharacterStatsEventsContainer.OnCharacterStatAdded -= AddStatServerRpc;
            CharacterStatsEventsContainer.OnCharacterStatRemoved -= MinusStatServerRpc;
        }

        private void InitStatsValue(CharacterStats characterStats)
        {
            if (!IsOwner) return;

            CharacterStatsEventsContainer.OnCharacterStatAdded += AddStatServerRpc;
            CharacterStatsEventsContainer.OnCharacterStatRemoved += MinusStatServerRpc;

            _food.OnValueChanged += (int oldValue, int newValue) =>
            {
                if (newValue <= 15)
                    AlertEventsContainer.OnStarvingAlert?.Invoke(true);
                else AlertEventsContainer.OnStarvingAlert?.Invoke(false);
            };

            _water.OnValueChanged += (int oldValue, int newValue) =>
            {
                if (newValue <= 15)
                    AlertEventsContainer.OnDehydratedAlert?.Invoke(true);
                else AlertEventsContainer.OnDehydratedAlert?.Invoke(false);
            };
        }

        [ServerRpc(RequireOwnership = false)]
        public void AddStatServerRpc(CharacterStatType type, int value)
        {
            if (!IsServer) return;
            PlusStatOnServer(type, value);
        }

        [ServerRpc(RequireOwnership = false)]
        public void MinusStatServerRpc(CharacterStatType type, int value)
        {
            if (!IsServer) return;
            MinusStatOnServer(type, value);
        }

        private int GetValidatedAddingStat(int stat, int addingValue)
        {
            var res = stat + addingValue;
            return res > 100 ? 100 : res;
        }

        private int GetValidatedRemovingStat(int stat, int removingValue)
        {
            var res = stat - removingValue;
            return res < 0 ? 0 : res;
        }

        private void PlusStatOnServer(CharacterStatType type, int value)
        {
            switch (type)
            {
                case CharacterStatType.Health:
                    _hp.Value = GetValidatedAddingStat(_hp.Value, value);
                    break;
                case CharacterStatType.Food:
                    _food.Value = GetValidatedAddingStat(_food.Value, value);
                    break;
                case CharacterStatType.Water:
                    _water.Value = GetValidatedAddingStat(_water.Value, value);
                    break;
                case CharacterStatType.Oxygen:
                    _oxygen.Value = GetValidatedAddingStat(_oxygen.Value, value);
                    break;
            }
        }

        [ClientRpc]
        private void ValidateHpClientRpc(int newHp)
        {
            if(!IsOwner) return;
            Debug.Log("Hp validation: " + newHp);
            if (newHp <= 0)
                PlayerNetCode.Singleton.PlayerKiller.DieServerRpc(UserDataHandler.Singleton.UserData.Id);
            else if (newHp <= 15)
                PlayerNetCode.Singleton.PlayerKnockDowner.KnockDownServerRpc(UserDataHandler.Singleton.UserData.Id);
        }

        private void MinusStatOnServer(CharacterStatType type, int value)
        {
            switch (type)
            {
                case CharacterStatType.Health:
                {
                    _hp.Value = GetValidatedRemovingStat(_hp.Value, value);
                    ValidateHpClientRpc(_hp.Value);
                    break;
                }
                case CharacterStatType.Food:
                    _food.Value = GetValidatedRemovingStat(_food.Value, value);
                    break;
                case CharacterStatType.Water:
                    _water.Value = GetValidatedRemovingStat(_water.Value, value);
                    break;
                case CharacterStatType.Oxygen:
                    _oxygen.Value = GetValidatedRemovingStat(_oxygen.Value, value);
                    break;
            }
        }
    }
}