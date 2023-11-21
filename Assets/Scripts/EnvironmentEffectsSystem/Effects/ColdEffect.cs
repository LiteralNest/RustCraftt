using System.Collections;
using UnityEngine;

namespace EnvironmentEffectsSystem.Effects
{
    public class ColdEffect : MonoBehaviour
    {
        [SerializeField] private float _coldEffectValue = 10f;
        [SerializeField] private float _temperatureDecreaseRate = 1f;
        [SerializeField] private float _temperatureDecreaseInterval = 2f;
        [SerializeField] private float _coldEffectInterval = 2f;

        private CharacterStats _characterStats;

        private bool _isEntering;
        private bool _isEffectActive;

        public void SetCharacterStats(CharacterStats characterStats)
        {
            _characterStats = characterStats;
        }

        public void OnEnter(TemperatureZone temperatureZone)
        {
            _isEntering = true;
            _isEffectActive = true;

            StartCoroutine(EffectByTimeRoutine(temperatureZone));
            Debug.Log("Entered Cold Zone");
        }

        public void OnExit()
        {
            _isEntering = false;
            _isEffectActive = false;
            
            // StartCoroutine(DecreaseEffectOverTime());
            Debug.Log("Exited Cold Zone");
        }

        private IEnumerator EffectByTimeRoutine(TemperatureZone temperatureZone)
        {
            while (_isEntering)
            {
                float currentTemperature = temperatureZone.GetTemperatureAtPosition(transform.position);
                currentTemperature -= _coldEffectValue;

                if (_isEffectActive && currentTemperature < -10f)
                {
                    _characterStats.MinusStat(CharacterStatType.Health, 1);
                }

                Debug.Log($"Current Temperature: {currentTemperature}");
                yield return new WaitForSeconds(_coldEffectInterval);
            }
        }

        // private IEnumerator DecreaseEffectOverTime()
        // {
        //     while (!_isEntering && _currentTemperature > 0f)
        //     {
        //         _currentTemperature -= _temperatureDecreaseRate;
        //         Debug.Log($"Current Temperature: {_currentTemperature}");
        //         yield return new WaitForSeconds(_temperatureDecreaseInterval);
        //     }
        // }
    }
}
