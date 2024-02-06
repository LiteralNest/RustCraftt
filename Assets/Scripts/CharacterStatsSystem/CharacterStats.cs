using AlertsSystem;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace CharacterStatsSystem
{
    public class CharacterStats : NetworkBehaviour
    {
        [field: SerializeField] public NetworkVariable<int> Hp { get; set; } = new NetworkVariable<int>(100);
        [field: SerializeField] public NetworkVariable<int> Food { get; set; } = new NetworkVariable<int>(100);
        [field: SerializeField] public NetworkVariable<int> Water { get; set; } = new NetworkVariable<int>(100);
        [field: SerializeField] public NetworkVariable<int> Oxygen { get; set; } = new NetworkVariable<int>(100);


        private void OnEnable()
            => CharacterStatsEventsContainer.OnCharacterStatsAssign += SetStatsValue;

        private void OnDisable()
            => CharacterStatsEventsContainer.OnCharacterStatsAssign -= SetStatsValue;

        private void SetStatsValue(CharacterStats characterStats)
        {
            Hp.OnValueChanged += (int oldValue, int newValue) =>
            {
                if (newValue <= 0)
                    PlayerNetCode.Singleton.PlayerKnockDowner.KnockDownServerRpc(UserDataHandler.Singleton.UserData.Id);
            };

            Food.OnValueChanged += (int oldValue, int newValue) =>
            {
                if (newValue <= 15)
                    AlertEventsContainer.OnStarvingAlert?.Invoke(true);
                else AlertEventsContainer.OnStarvingAlert?.Invoke(false);
            };

            Water.OnValueChanged += (int oldValue, int newValue) =>
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
                    Hp.Value = GetValidatedAddingStat(Hp.Value, value);
                    break;
                case CharacterStatType.Food:
                    Food.Value = GetValidatedAddingStat(Food.Value, value);
                    break;
                case CharacterStatType.Water:
                    Water.Value = GetValidatedAddingStat(Water.Value, value);
                    break;
                case CharacterStatType.Oxygen:
                    Oxygen.Value = GetValidatedAddingStat(Oxygen.Value, value);
                    break;
            }
        }

        private void MinusStat(CharacterStatType type, int value)
        {
            switch (type)
            {
                case CharacterStatType.Health:
                    Hp.Value = GetValidatedRemovingStat(Hp.Value, value);
                    if (Hp.Value <= 0)
                        PlayerNetCode.Singleton.PlayerKnockDowner.KnockDownServerRpc(UserDataHandler.Singleton.UserData
                            .Id);
                    break;
                case CharacterStatType.Food:
                    Food.Value = GetValidatedRemovingStat(Food.Value, value);
                    if (Food.Value <= 15)
                        AlertEventsContainer.OnStarvingAlert?.Invoke(true);
                    else
                        AlertEventsContainer.OnStarvingAlert?.Invoke(false);
                    break;
                case CharacterStatType.Water:
                    Water.Value = GetValidatedRemovingStat(Water.Value, value);
                    if (Water.Value <= 15)
                        AlertEventsContainer.OnDehydratedAlert?.Invoke(true);
                    else
                        AlertEventsContainer.OnDehydratedAlert?.Invoke(false);
                    break;
                case CharacterStatType.Oxygen:
                    Oxygen.Value = GetValidatedRemovingStat(Oxygen.Value, value);
                    break;
            }
        }
    }
}