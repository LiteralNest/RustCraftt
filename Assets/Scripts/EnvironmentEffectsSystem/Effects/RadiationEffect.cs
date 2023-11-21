using System.Collections;
using UnityEngine;

namespace EnvironmentEffectsSystem.Effects
{
    public class RadiationEffect : MonoBehaviour
    {
        [SerializeField] private float _radiationEffectValue = 1f;
        [SerializeField] private float _radiationEffectInterval = 2f;
        [SerializeField] private float _radiationDecreaseRate = 1f;
        [SerializeField] private float _radiationDecreaseInterval = 2f;

        private const float RadiationCritLevel = 10;
        private float _currentRadiationLevel = 0f;
        
        private CharacterStats _characterStats;
        private bool _isEffectActive = false;
        private bool _isEnteringZone = false;
        

        public void SetCharacterStats(CharacterStats characterStats)
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
            _isEffectActive = true;
            _isEnteringZone = true;
            
            StartCoroutine(EffectByTime());
        }

        public void OnExit()
        {
            Debug.Log("Exited Radiation Zone");
            _isEnteringZone = false;
            _isEffectActive = false;
            
            StartCoroutine(DecreaseEffectOverTime());

            if (_currentRadiationLevel == 0)
            {
                Debug.Log("Radiation is zero");
            }
        }
        

        private IEnumerator EffectByTime()
        {
            while (_isEffectActive)
            {
                _currentRadiationLevel += _radiationEffectValue;

                if (_isEnteringZone && _characterStats != null && _currentRadiationLevel >= RadiationCritLevel)
                {
                    _characterStats.MinusStat(CharacterStatType.Health, Random.Range(7, 11));
                }

                Debug.Log($"Current Radiation Level: {_currentRadiationLevel}");
                yield return new WaitForSeconds(_radiationEffectInterval);
            }
        }

        public IEnumerator DecreaseEffectOverTime()
        {
            while (!_isEnteringZone && _currentRadiationLevel > 0f)
            {
                _currentRadiationLevel -= _radiationDecreaseRate;

                if (!_isEnteringZone && _currentRadiationLevel >= RadiationCritLevel / 2)
                {
                    _characterStats.MinusStat(CharacterStatType.Health, Random.Range(3, 6));
                }

                Debug.Log($"Current Radiation Level: {_currentRadiationLevel}");
                yield return new WaitForSeconds(_radiationDecreaseInterval);
            }
        }
    }
}
