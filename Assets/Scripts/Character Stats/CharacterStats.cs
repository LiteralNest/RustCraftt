using UniRx;
using Events;
using UnityEngine;

namespace Character_Stats
{
    [RequireComponent(typeof(CharacterStatsView))]
    public class CharacterStats : MonoBehaviour
    {
        [SerializeField] private ReactiveProperty<float>  _hp = 100f;
        [SerializeField] private float _food = 100f;
        [SerializeField] private float _water = 100f;
        [SerializeField] private float _oxygen = 100f;

        
        
        private CharacterStatsView _statsView;

        private void Awake()
        {
            if (_statsView == null)
                _statsView = GetComponent<CharacterStatsView>();
        }

        private void Start()
            => _statsView.DisplayHp(_hpHandler.Hp);

        public void AssignHp(int value)
        {
            _hpHandler.AssignHpServerRpc(value);
            _statsView.DisplayHp(value);
        }

        public void DisplayHp(int value)
            => _statsView.DisplayHp(value);

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
                    _food = GetAddedStat(_food, value);
                    if (_statsView != null)
                        _statsView.DisplayFood((int)_food);
                    break;
                case CharacterStatType.Water:
                    _water = GetAddedStat(_water, value);
                    if (_statsView != null)
                        _statsView.DisplayWater((int)_water);
                    break;
                case CharacterStatType.Oxygen:
                    _oxygen = GetAddedStat(_oxygen, value);
                    if (_statsView != null)
                        _statsView.DisplayOxygen((int)_oxygen);
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
                    _food = GetSubstractedStat(_food, value);
                    _statsView.DisplayFood((int)_food);
                    break;
                case CharacterStatType.Water:
                    _water = GetSubstractedStat(_water, value);
                    _statsView.DisplayWater((int)_water);
                    break;
                case CharacterStatType.Oxygen:
                    _oxygen = GetSubstractedStat(_oxygen, value);
                    _statsView.DisplayOxygen((int)_oxygen);
                    break;
            }
        }
    }
}