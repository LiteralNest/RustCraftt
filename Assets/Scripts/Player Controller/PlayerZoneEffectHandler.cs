using EnvironmentEffectsSystem.Effects;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerZoneEffectHandler : MonoBehaviour
    {
        private ColdEffect _currentColdEffect;
        private RadiationEffect _currentRadiationEffect;
        private CharacterStats _characterStats;

        private void Start()
        {
            _characterStats = CharacterStats.Singleton;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ColdEnvironment"))
            {
                ColdEffect coldEffect = other.GetComponent<ColdEffect>();
                if (coldEffect != null)
                {
                    _currentColdEffect = coldEffect;
                    _currentColdEffect.SetCharacterStats(_characterStats);
                    _currentColdEffect.OnEnter(transform);
                }
            }
            if (other.CompareTag("RadioactiveEnvironment"))
            {
                RadiationEffect radiationEffect = other.GetComponent<RadiationEffect>();
                if (radiationEffect != null)
                {
                    _currentRadiationEffect = radiationEffect;
                    _currentRadiationEffect.SetCharacterStats(_characterStats);
                    _currentRadiationEffect.OnEnter();
                }
            }
        }

        // private void OnTriggerStay(Collider other)
        // {
        //     if (_currentColdEffect != null && _currentColdEffect.MatchesTrigger(other))
        //     {
        //         _currentColdEffect.OnStay();
        //     }
        // }

        private void OnTriggerExit(Collider other)
        {
            if (_currentColdEffect != null && _currentColdEffect.MatchesTrigger(other))
            {
                _currentColdEffect.OnExit(transform);
                _currentColdEffect = null;
            }
            if (_currentRadiationEffect != null && _currentRadiationEffect.MatchesTrigger(other))
            {
                _currentRadiationEffect.OnExit();
                _currentRadiationEffect = null;
            }
        }
    }
}
