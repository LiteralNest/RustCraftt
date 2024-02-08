using AlertsSystem;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

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
            CharacterStatsEventsContainer.OnCharacterStatAdded += AddStatServerRpc;
            CharacterStatsEventsContainer.OnCharacterStatRemoved += MinusStatServerRpc;


            _hp.OnValueChanged += (int oldValue, int newValue) =>
            {
                if (newValue <= 0)
                    PlayerNetCode.Singleton.PlayerKnockDowner.KnockDownServerRpc(UserDataHandler.Singleton.UserData.Id);
            };

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
            PlusStat(type, value);
        }

        [ServerRpc(RequireOwnership = false)]
        public void MinusStatServerRpc(CharacterStatType type, int value)
        {
            if (!IsServer) return;
            MinusStat(type, value);
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

        private void PlusStat(CharacterStatType type, int value)
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

        private void MinusStat(CharacterStatType type, int value)
        {
            switch (type)
            {
                case CharacterStatType.Health:
                    _hp.Value = GetValidatedRemovingStat(_hp.Value, value);
                    break;
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