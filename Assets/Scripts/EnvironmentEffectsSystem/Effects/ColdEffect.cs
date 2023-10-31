using System.Collections;
using SurroundingEffectsSystem;
using UnityEngine;

namespace EnvironmentEffectsSystem.Effects
{
    public class ColdEffect : IEnvironmentEffect
    {
        //[SerializeField] 
        private readonly float _coldEffectValue = 10f;
        //[SerializeField] 
        private readonly float _temperatureDecreaseRate = 1f;
        //[SerializeField] 
        private readonly float _temperatureDecreaseInterval = 2f;
        //[SerializeField] 
        private readonly float _coldEffectInterval = 2f;

        private float _currentTemperature;
        private CharacterStats _characterStats;

        public ColdEffect(CharacterStats characterStats)
        {
            _characterStats = characterStats;
        }

        public bool MatchesTrigger(Collider other)
        {
            return other.CompareTag("ColdEnvironment");
        }

        public void OnEnter()
        {
            Debug.Log("Entered Cold Zone");
        }

        public void OnExit()
        {
            Debug.Log("Exited Cold Zone");
         
        }

        public IEnumerator EffectByTime(Collider collider)
        {
            var match = MatchesTrigger(collider);
            while (match)
            {
                _currentTemperature -= _coldEffectValue;
                // DisplayTemperature();

                if (_currentTemperature < -10f)
                {
                    _characterStats.MinusStat(CharacterStatType.Health, 1);
                }

                Debug.Log($"Current Temperature: {_currentTemperature}");
                yield return new WaitForSeconds(_coldEffectInterval);

                match = MatchesTrigger(collider);
            }
        }

        public IEnumerator DecreaseEffectOverTime()
        {
            while (_currentTemperature > 0f)
            {
                _currentTemperature -= _temperatureDecreaseRate;
                Debug.Log($"Current Temperature: {_currentTemperature}");
                yield return new WaitForSeconds(_temperatureDecreaseInterval);
            }
        }
    }
}