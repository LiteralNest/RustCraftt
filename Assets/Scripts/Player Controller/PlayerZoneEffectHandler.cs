using EnvironmentEffectsSystem.Effects;
using SurroundingEffectsSystem;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerZoneEffectHandler : MonoBehaviour
    {
        private IEnvironmentEffect _currentEffect;
        private CharacterStats _characterStats;

        [SerializeField] private ColdEffect _coldEffect;
        
        private void Start()
        {
            _characterStats = CharacterStats.Singleton;
        }

        private void OnTriggerEnter(Collider other)
        {
            IEnvironmentEffect effect = null;

            if (other.CompareTag("ColdEnvironment"))
            {
                effect = _coldEffect;
            }
            // else if (other.CompareTag("RadioactiveEnvironment"))
            // {
            //     effect = new RadiationEffect(_characterStats);
            // }

            if (effect != null && effect != _currentEffect)
            {
                effect.OnEnter();
                _currentEffect = effect;
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