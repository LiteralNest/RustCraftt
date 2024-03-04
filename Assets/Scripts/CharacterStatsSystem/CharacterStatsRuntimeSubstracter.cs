using System.Collections;
using UnityEngine;

namespace CharacterStatsSystem
{
    public class CharacterStatsRuntimeSubstracter : MonoBehaviour
    {
        [Header("Main Params")] [Range(1, 100)] [SerializeField]
        private float _timeForSubstractFood = 2f;

        [Range(1, 100)] [SerializeField] private float _timeForSubstractWater = 2.5f;
        [Range(1, 100)] [SerializeField] private float _timeForSubstractHp = 5f;

        private CharacterStats _characterStats;

        private Coroutine _substractHpRoutine;
        private Coroutine _substractFoodRoutine;
        private Coroutine _substractWaterRoutine;

        private void OnEnable()
            => CharacterStatsEventsContainer.OnCharacterStatsAssign += Init;

        private void OnDisable()
            => CharacterStatsEventsContainer.OnCharacterStatsAssign -= Init;

        private void Init(CharacterStats characterStats)
        {
            _characterStats = characterStats;
            if (_substractFoodRoutine != null)
                StopCoroutine(_substractFoodRoutine);
            _substractFoodRoutine = StartCoroutine(SubstractFoodRoutine());
            if (_substractWaterRoutine != null)
                StopCoroutine(_substractWaterRoutine);
            _substractWaterRoutine = StartCoroutine(SubstractWaterRoutine());
            _characterStats.Water.OnValueChanged += (int oldValue, int newValue) => CheckWater(newValue);
            _characterStats.Food.OnValueChanged += (int oldValue, int newValue) => CheckFood(newValue);
        }

        private IEnumerator SubstractFoodRoutine()
        {
            yield return new WaitForSeconds(_timeForSubstractFood);
            if (_characterStats.Food.Value > 0)
                CharacterStatsEventsContainer.OnCharacterStatRemoved(CharacterStatType.Food, 1);
            _substractFoodRoutine = StartCoroutine(SubstractFoodRoutine());
        }

        private IEnumerator SubstractWaterRoutine()
        {
            yield return new WaitForSeconds(_timeForSubstractWater);
            if (_characterStats.Water.Value > 0)
                CharacterStatsEventsContainer.OnCharacterStatRemoved(CharacterStatType.Water, 1);
            _substractWaterRoutine = StartCoroutine(SubstractWaterRoutine());
        }

        private IEnumerator SubstractHpRoutine()
        {
            yield return new WaitForSeconds(_timeForSubstractHp);
            if (_characterStats.Hp.Value > 0)
                CharacterStatsEventsContainer.OnCharacterStatRemoved(CharacterStatType.Health, 1);
            _substractHpRoutine = StartCoroutine(SubstractHpRoutine());
        }

        private void CheckFood(int value)
        {
            if (value <= 15)
            {
                if (_substractHpRoutine == null)
                    _substractHpRoutine = StartCoroutine(SubstractHpRoutine());
            }
            else
            {
                if (_substractHpRoutine != null)
                    StopCoroutine(_substractHpRoutine);
            }
        }

        private void CheckWater(int value)
        {
            if (value <= 15)
            {
                if (_substractFoodRoutine == null)
                    _substractFoodRoutine = StartCoroutine(SubstractFoodRoutine());
            }
            else
            {
                if (_substractHpRoutine != null)
                    StopCoroutine(_substractHpRoutine);
            }
        }
    }
}