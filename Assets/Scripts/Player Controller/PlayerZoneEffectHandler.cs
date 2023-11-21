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
                var coldEffect = other.GetComponent<ColdEffect>();
                var temperatureZone = other.GetComponent<TemperatureZone>();
                
                if (coldEffect != null)
                {
                    _currentColdEffect = coldEffect;
                    _currentColdEffect.SetCharacterStats(_characterStats);
                    _currentColdEffect.OnEnter(temperatureZone);
                }
            }
            else if (other.CompareTag("RadioactiveEnvironment"))
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

        private void OnTriggerExit(Collider other)
        {
            if (_currentColdEffect != null && _currentColdEffect.MatchesTrigger(other))
            {
                _currentColdEffect.OnExit();
                _currentColdEffect = null;
            }
            else if (_currentRadiationEffect != null && _currentRadiationEffect.MatchesTrigger(other))
            {
                _currentRadiationEffect.OnExit();
                _currentRadiationEffect = null;
            }
        }
    }
}