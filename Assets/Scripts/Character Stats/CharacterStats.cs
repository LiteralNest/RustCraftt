using Events;
using UnityEngine;

namespace Character_Stats
{
    public class CharacterStats : MonoBehaviour
    {
        public static CharacterStats Singleton { get; private set; }

        [SerializeField] private CharacterStatsDisplayer _statsDisplayer;
        [SerializeField] private CharacterHpHandler _hpHandler;
        [field: SerializeField] public float Food { get; private set; }
        [field: SerializeField] public float Water { get; private set; }
        [field: SerializeField] public float Oxygen { get; private set; }

        [SerializeField] private GameObject _OxygenPanel;

        public float CurrentOxygen { get; private set; }

        private void Awake()
        {
            if (_statsDisplayer == null)
                _statsDisplayer = GetComponent<CharacterStatsDisplayer>();
            Singleton = this;

            CurrentOxygen = Oxygen;
        }

        private void Start()
            => _statsDisplayer.DisplayHp(_hpHandler.Hp);

        public void AssignHp(int value)
        {
            _hpHandler.AssignHpServerRpc(value);
            _statsDisplayer.DisplayHp(value);
        }

        public void DisplayHp(int value)
            => _statsDisplayer.DisplayHp(value);
        
        private float GetAddedStat(float stat, float addingValue)
        {
            float res = stat + addingValue;
            if (res > 100)
                res = 100;
            return res;
        }

        public void PlusStat(CharacterStatType type, float value)
        {
            switch (type)
            {
                case CharacterStatType.Health:
                    _hpHandler.AddHpServerRpc((int)value);
                    break;
                case CharacterStatType.Food:
                    Food = GetAddedStat(Food, value);
                    if (_statsDisplayer != null)
                        _statsDisplayer.DisplayFood((int)Food);
                    break;
                case CharacterStatType.Water:
                    Water = GetAddedStat(Water, value);
                    if (_statsDisplayer != null)
                        _statsDisplayer.DisplayWater((int)Water);
                    break;
                case CharacterStatType.Oxygen:
                    Oxygen = GetAddedStat(Oxygen, value);
                    if (_statsDisplayer != null)
                        _statsDisplayer.DisplayOxygen((int)Oxygen);
                    break;
            }
            GlobalEventsContainer.CharacterStatsChanged?.Invoke();
        }

        private float GetSubstractedStat(float stat, float substractingValue)
        {
            float res = stat - substractingValue;
            if (res < 0)
                res = 0;
            return res;
        }

        public void MinusStat(CharacterStatType type, float value)
        {
            switch (type)
            {
                case CharacterStatType.Health:
                    _hpHandler.GetDamageServerRpc((int)value);
                    break;
                case CharacterStatType.Food:
                    Food = GetSubstractedStat(Food, value);
                    _statsDisplayer.DisplayFood((int)Food);
                    if (Food <= 0)
                    {
                        // GlobalEventsContainer.PlayerDied?.Invoke();
                        // _statsDisplayer.DisplayDeathMessage("You died!", Color.green);
                    }

                    break;
                case CharacterStatType.Water:
                    Water = GetSubstractedStat(Water, value);
                    _statsDisplayer.DisplayWater((int)Water);
                    if (Water <= 0)
                    {
                        // GlobalEventsContainer.PlayerDied?.Invoke();
                        // _statsDisplayer.DisplayDeathMessage("You died!", Color.blue);
                    }

                    break;
                case CharacterStatType.Oxygen:
                    Oxygen = GetSubstractedStat(Oxygen, value);
                    _statsDisplayer.DisplayOxygen((int)Oxygen);

                    if (Oxygen < 0)
                    {
                    }

                    break;
            }
        }

        public void SetActiveOxygen(bool state)
        {
            _OxygenPanel.SetActive(state);
        }
    }
}