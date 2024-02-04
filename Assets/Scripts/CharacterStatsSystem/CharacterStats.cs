using Unity.Netcode;
using UnityEngine;

namespace CharacterStatsSystem
{
    public class CharacterStats : NetworkBehaviour
    {
        [field: SerializeField] public NetworkVariable<int> Hp { get; set; } = new NetworkVariable<int>(100);
        [field: SerializeField] public NetworkVariable<int> Food { get; set; } = new NetworkVariable<int>(100);
        [field: SerializeField] public NetworkVariable<int> Water { get; set; } = new NetworkVariable<int>(100);
        [field: SerializeField] public NetworkVariable<int> Oxygen { get; set; } = new NetworkVariable<int>(100);

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
                    break;
                case CharacterStatType.Food:
                    Food.Value = GetValidatedRemovingStat(Food.Value, value);
                    break;
                case CharacterStatType.Water:
                    Water.Value = GetValidatedRemovingStat(Water.Value, value);
                    break;
                case CharacterStatType.Oxygen:
                    Oxygen.Value = GetValidatedRemovingStat(Oxygen.Value, value);
                    break;
            }
        }
    }
}