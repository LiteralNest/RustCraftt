using System.Collections;
using Alerts_System.Alerts;
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
 

        public bool MatchesTrigger(Collider other) => other.CompareTag("RadioactiveEnvironment");

        public void SetCharacterStats(CharacterStats characterStats)
        {
            _characterStats = characterStats;
        }

        public void OnEnter(float resist)
        {
            Debug.Log("Entered Radiation Zone");
            GlobalEventsContainer.RadiationStarted?.Invoke();
            _isEffectActive = true;
            _isEnteringZone = true;
            
            StartCoroutine(EffectByTime(resist));
        }

        public void OnExit(float resist)
        {
            Debug.Log("Exited Radiation Zone");
            GlobalEventsContainer.RadiationEnded?.Invoke();
            _isEnteringZone = false;
            _isEffectActive = false;
            
            StartCoroutine(DecreaseEffectOverTime(resist));

            if (_currentRadiationLevel == 0)
            {
                Debug.Log("Radiation is zero");
            }
        }
        

        private IEnumerator EffectByTime(float resist)
        {
            while (_isEffectActive)
            {
                _currentRadiationLevel += _radiationEffectValue;
                AlertsDisplayer.Singleton.DisplayRadiationAlert((int)_currentRadiationLevel);
                if (_isEnteringZone && _characterStats != null && _currentRadiationLevel >= RadiationCritLevel)
                {
                    _characterStats.MinusStat(CharacterStatType.Health, Random.Range(7, 11) * resist);
                }

                Debug.Log($"Current Radiation Level: {_currentRadiationLevel}");
                yield return new WaitForSeconds(_radiationEffectInterval);
            }
        }

        public IEnumerator DecreaseEffectOverTime(float resist)
        {
            while (!_isEnteringZone && _currentRadiationLevel > 0f)
            {
                _currentRadiationLevel -= _radiationDecreaseRate;

                if (!_isEnteringZone && _currentRadiationLevel >= RadiationCritLevel / 2)
                {
                    _characterStats.MinusStat(CharacterStatType.Health, Random.Range(3, 6) * resist);
                }

                Debug.Log($"Current Radiation Level: {_currentRadiationLevel}");
                yield return new WaitForSeconds(_radiationDecreaseInterval);
            }
            AlertsDisplayer.Singleton.DisplayRadiationAlert((int)_currentRadiationLevel, false);
        }
    }
}
