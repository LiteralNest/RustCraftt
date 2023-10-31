using EnvironmentEffectsSystem.Effects;
using SurroundingEffectsSystem;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerZoneEffectHandler : MonoBehaviour
    {
        private IEnvironmentEffect _currentEffect;
        private CharacterStats _characterStats;
      
        private void Start()
        {
            _characterStats = CharacterStats.Singleton;
        }

        private void OnTriggerEnter(Collider other)
        {
            IEnvironmentEffect effect = null;

            if (other.CompareTag("ColdEnvironment"))
            {
                effect = new ColdEffect(_characterStats);
            }
            else if (other.CompareTag("RadioactiveEnvironment"))
            {
                effect = new RadiationEffect(_characterStats);
            }

            if (effect != null && effect != _currentEffect)
            {
                _currentEffect = effect;
                StartCoroutine(effect.EffectByTime(other));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _currentEffect.OnExit();
            if (_currentEffect != null && _currentEffect.MatchesTrigger(other))
            {
                _currentEffect = null;

                StartCoroutine(_currentEffect.DecreaseEffectOverTime());
            }
        }
    }
}