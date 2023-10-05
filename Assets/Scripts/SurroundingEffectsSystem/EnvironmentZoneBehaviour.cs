using UnityEngine;
using System.Collections;

namespace SurroundingEffectsSystem
{
    public class EnvironmentZoneBehaviour : MonoBehaviour
    {
        [SerializeField] private float _coldEffectValue = 0.05f;
        [SerializeField] private float _warmEffectValue = 0.05f;
        [SerializeField] private float _radiationEffectValue = 0.001f;
        [SerializeField] private float _radiationDamageOverTime = 0.01f; 
        [SerializeField] private float _radiationDamageInterval = 2f;
        [SerializeField] private float _coldEffectInterval = 2f;
        [SerializeField] private float _warmEffectInterval = 2f;

        private EnvironmentEffectsStateType _currentEffect;
        private CharacterStats _characterStats;

        private void Start()
        {
            _characterStats = CharacterStats.Singleton;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("ColdEnvironment"))
            {
                SetEffect(EnvironmentEffectsStateType.Cold);
                StartCoroutine(ColdEffect());
            }
            else if (other.CompareTag("WarmEnvironment"))
            {
                SetEffect(EnvironmentEffectsStateType.Warm);
                StartCoroutine(WarmEffect());
            }
            else if (other.CompareTag("RadioactiveEnvironment"))
            {
                SetEffect(EnvironmentEffectsStateType.Radiation);
                StartCoroutine(RadiationEffect());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ColdEnvironment") && _currentEffect == EnvironmentEffectsStateType.Cold)
            {
                SetEffect(EnvironmentEffectsStateType.None);
            }
            else if (other.CompareTag("WarmEnvironment") && _currentEffect == EnvironmentEffectsStateType.Warm)
            {
                SetEffect(EnvironmentEffectsStateType.None);
            }
            else if (other.CompareTag("RadioactiveEnvironment") && _currentEffect == EnvironmentEffectsStateType.Radiation)
            {
                SetEffect(EnvironmentEffectsStateType.None);
            }
        }

        private void SetEffect(EnvironmentEffectsStateType effect)
        {
            if (_currentEffect == effect)
            {
                return;
            }

            _currentEffect = effect;

            GlobalEventsContainer.TemperatureChanged?.Invoke(effect);

            switch (effect)
            {
                case EnvironmentEffectsStateType.Critical:
                    GlobalEventsContainer.CriticalTemperatureReached?.Invoke();
                    break;
                case EnvironmentEffectsStateType.Radiation:
                    GlobalEventsContainer.RadiationStarted?.Invoke();
                    break;
                case EnvironmentEffectsStateType.Cold:
                    GlobalEventsContainer.TemperatureChanged?.Invoke(EnvironmentEffectsStateType.Cold);
                    StartCoroutine(ColdEffect());
                    break;
                case EnvironmentEffectsStateType.Warm:
                    GlobalEventsContainer.TemperatureChanged?.Invoke(EnvironmentEffectsStateType.Warm);
                    StartCoroutine(WarmEffect());
                    break;
                case EnvironmentEffectsStateType.None:
                    GlobalEventsContainer.RadiationEnded?.Invoke();
                    break;
            }
        }

        private IEnumerator ColdEffect()
        {
            while (_currentEffect == EnvironmentEffectsStateType.Cold)
            {
                _characterStats.MinusStat(CharacterStatType.Health, _coldEffectValue);
                yield return new WaitForSeconds(_coldEffectInterval);
            }
        } 
        
        private IEnumerator WarmEffect()
        {
            while (_currentEffect == EnvironmentEffectsStateType.Warm)
            {
                _characterStats.PlusStat(CharacterStatType.Health, _warmEffectValue);
                yield return new WaitForSeconds(_warmEffectInterval);
            }
        }
        
        private IEnumerator RadiationEffect()
        {
            float radiationValue = _radiationEffectValue;
            
            _characterStats.MinusStat(CharacterStatType.Health, radiationValue);
            
            if (radiationValue >= 100f)
            {
                yield return StartCoroutine(RadiationDamageOverTime());
            }

            Debug.Log($"Radiation Effect Applied. Health Decreased by {radiationValue}");
        }

        private IEnumerator RadiationDamageOverTime()
        {
            while (_currentEffect == EnvironmentEffectsStateType.Radiation)
            {
                _characterStats.MinusStat(CharacterStatType.Health, _radiationDamageOverTime);
                yield return new WaitForSeconds(_radiationDamageInterval);
            }
        }
    }
}
