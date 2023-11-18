using System.Collections;
using System.Threading.Tasks;
using SurroundingEffectsSystem;
using UnityEngine;

namespace EnvironmentEffectsSystem.Effects
{
    public class ColdEffect : MonoBehaviour ,IEnvironmentEffect
    {
        private readonly float _coldEffectValue = 10f;
        private readonly float _temperatureDecreaseRate = 1f;
        private readonly float _temperatureDecreaseInterval = 2f;
        private readonly float _coldEffectInterval = 2f;

        private float _currentTemperature;
        private CharacterStats _characterStats;

        private bool _isEntering;
        
        public bool MatchesTrigger(Collider other)
        { 
            return other.CompareTag("ColdEnvironment");
        }
        
        
        public void OnEnter()
        {
            _isEntering = true;
            StartCoroutine(EffectByTimeRoutine());
            Debug.Log("Entered Cold Zone");
        }

        public void OnExit()
        {
            _isEntering = false;
            StartCoroutine(DecreaseEffectOverTime());
            Debug.Log("Exited Cold Zone");
        }


        private IEnumerator EffectByTimeRoutine()
        {
            while (_isEntering)
            {
                _currentTemperature -= _coldEffectValue;
                // DisplayTemperature();

                if (_currentTemperature < -10f)
                {
                    _characterStats.MinusStat(CharacterStatType.Health, 1);
                }

                Debug.Log($"Current Temperature: {_currentTemperature}");
                yield return new WaitForSeconds(_coldEffectInterval);
            }
        }

        private IEnumerator DecreaseEffectOverTime()
        {
            while (!_isEntering && _currentTemperature > 0f)
            {
                _currentTemperature -= _temperatureDecreaseRate;
                Debug.Log($"Current Temperature: {_currentTemperature}");
                yield return new WaitForSeconds(_temperatureDecreaseInterval);
            }
        }
    }
}