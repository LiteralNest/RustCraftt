using System.Collections;
using Alerts_System.Alerts;
using Events;
using UnityEngine;

namespace Character_Stats
{
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterStatsRuntimeSubstracter : MonoBehaviour
    {
        [Header("Main Params")] [Range(1, 100)] [SerializeField]
        private float _timeForMinusingOneStat = 3f;

        [Range(1, 100)] [SerializeField] private float _timeForMinusingHp = 5f;

        private Coroutine _minusHpRoutine;
        private Coroutine _minusStatsRoutine;

        private CharacterStats _characterStats;

        private void OnEnable()
            => GlobalEventsContainer.CharacterStatsChanged += CheckStats;

        private void OnDisable()
            => GlobalEventsContainer.CharacterStatsChanged -= CheckStats;

        private void Start()
        {
            _characterStats = GetComponent<CharacterStats>();
            _minusStatsRoutine = StartCoroutine(SubstractStatsRoutine());
        }

        private IEnumerator SubstractStatsRoutine()
        {
            yield return new WaitForSeconds(_timeForMinusingOneStat);
            _characterStats.MinusStat(CharacterStatType.Food, 1);
            _characterStats.MinusStat(CharacterStatType.Water, 1);
            CheckStats();
        }

        private void CheckStats()
        {
            if (_characterStats.Food <= 15)
                AlertsDisplayer.Singleton.DisplayStarvingAlert(true);
            else
                AlertsDisplayer.Singleton.DisplayStarvingAlert(false);

            if (_characterStats.Water <= 15)
                AlertsDisplayer.Singleton.DisplayDehydratedAlert(true);
            else
                AlertsDisplayer.Singleton.DisplayDehydratedAlert(false);

            if (_minusStatsRoutine != null)
                StopCoroutine(_minusStatsRoutine);
            if (_minusHpRoutine != null)
                StopCoroutine(_minusHpRoutine);

            if (_characterStats.Food <= 0 || _characterStats.Water <= 0)
                _minusHpRoutine = StartCoroutine(MinusHpRoutine());
            else
                _minusStatsRoutine = StartCoroutine(SubstractStatsRoutine());
        }

        private IEnumerator MinusHpRoutine()
        {
            yield return new WaitForSeconds(_timeForMinusingHp);
            _characterStats.MinusStat(CharacterStatType.Health, 1);
            _minusHpRoutine = StartCoroutine(MinusHpRoutine());
        }
    }
}