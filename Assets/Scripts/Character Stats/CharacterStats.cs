using UnityEngine;

namespace Character_Stats
{
    public class CharacterStats : MonoBehaviour
    {
        public static CharacterStats Singleton { get; private set; }

        [SerializeField] private CharacterStatsDisplayer _statsDisplayer;

        [field: SerializeField] public float Health { get; private set; }
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
            => _statsDisplayer.DisplayHp((int)Health);

        public void AssignHp(int value)
        {
            Health = value;
            _statsDisplayer.DisplayHp(value);
        }

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
                    Health = GetAddedStat(Health, value);
                    if (_statsDisplayer != null)
                        _statsDisplayer.DisplayHp((int)Health);
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
                    Health = GetSubstractedStat(Health, value);
                    _statsDisplayer.DisplayHp((int)Health);
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