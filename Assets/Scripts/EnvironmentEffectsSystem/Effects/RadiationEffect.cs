using System.Collections;
using SurroundingEffectsSystem;
using UnityEngine;

namespace EnvironmentEffectsSystem.Effects
{
    public class RadiationEffect// : IEnvironmentEffect
    {
        [SerializeField] private float _radiationEffectValue = 1f;
        [SerializeField] private float _radiationEffectInterval = 2f;
        [SerializeField] private float _radiationDecreaseRate = 1f; // Изменено
        [SerializeField] private float _radiationDecreaseInterval = 2f; // Изменено
        
        private float _currentRadiationLevel = 0f;
        private CharacterStats _characterStats;
        
        public RadiationEffect(CharacterStats characterStats)
        {
            _characterStats = characterStats;
        }
        
        public bool MatchesTrigger(Collider other)
        {
            return other.CompareTag("RadioactiveEnvironment");
        }

        public void OnEnter()
        {
            Debug.Log("Entered Radiation Zone");
            // DisplayRadiationLevel();
        }

        public void OnExit()
        {
            if (_currentRadiationLevel == 0)
            {
                Debug.Log("Exited Radiation Zone");
                // HideRadiationLevel();
            }
        }

        public IEnumerator EffectByTime(Collider collider)
        {
            var match = MatchesTrigger(collider);
            while (match)
            {
                _currentRadiationLevel += _radiationEffectValue;

                if (_currentRadiationLevel >= 10f)
                {
                    _characterStats.MinusStat(CharacterStatType.Health, Random.Range(7, 11));
                }

                Debug.Log($"Current Radiation Level: {_currentRadiationLevel}");
                yield return new WaitForSeconds(_radiationEffectInterval);
                
                match = MatchesTrigger(collider);
            }

          
        }

        public IEnumerator DecreaseEffectOverTime()
        {
            while (_currentRadiationLevel > 0f)
            {
                _currentRadiationLevel -= _radiationDecreaseRate;
                // DisplayRadiationLevel()
                Debug.Log($"Current Radiation Level: {_currentRadiationLevel}");
                yield return new WaitForSeconds(_radiationDecreaseInterval); // Изменено
            }
        }
    }
}
