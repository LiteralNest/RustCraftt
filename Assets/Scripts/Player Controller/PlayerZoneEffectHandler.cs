// PlayerZoneEffectHandler.cs

using EnvironmentEffectsSystem.Effects;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerZoneEffectHandler : MonoBehaviour
    {
        private RadiationEffect _currentEffect;
        private CharacterStats _characterStats;

        private void Start()
        {
            _characterStats = CharacterStats.Singleton;
        }

        private void OnTriggerEnter(Collider other)
        {
            RadiationEffect effect = null;

            if (other.CompareTag("ColdEnvironment"))
            {
                //add cold effect
                
                // effect = other.GetComponent<RadiationEffect>();
                // if (effect != null)
                // {
                //     _currentEffect = effect;
                //     _currentEffect.SetCharacterStats(_characterStats);
                //     _currentEffect.OnEnter();
                // }
            }
            else if (other.CompareTag("RadioactiveEnvironment"))
            {
                effect = other.GetComponent<RadiationEffect>();
                if (effect != null)
                {
                    _currentEffect = effect;
                    _currentEffect.SetCharacterStats(_characterStats);
                    _currentEffect.OnEnter();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_currentEffect != null && _currentEffect.MatchesTrigger(other))
            {
                _currentEffect.OnExit();
                _currentEffect = null;
            }
        }
    }
}