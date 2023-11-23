using EnvironmentEffectsSystem.Effects;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerZoneEffectHandler : MonoBehaviour
    {
        private ColdEffect _currentColdEffect;
        private WarmEffect _currentWarmEffect;
        private RadiationEffect _currentRadiationEffect;
        private CharacterStats _characterStats;

        private PlayerResistParams _resist;
        private void Start()
        {
            _characterStats = CharacterStats.Singleton;
            _resist = GetComponent<PlayerResistParams>();
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
                    _currentColdEffect.OnEnter(transform, _resist.ColdResist);
                }
            }
            else if (other.CompareTag("WarmEnvironment"))
            {
                WarmEffect warmEffect = other.GetComponent<WarmEffect>();
                if (warmEffect != null)
                {
                    // Stop the cold effect if it's active
                    if (_currentColdEffect != null)
                    {
                        _currentColdEffect.OnExit(transform, _resist.ColdResist);
                    }

                    _currentWarmEffect = warmEffect;
                    _currentWarmEffect.SetCharacterStats(_characterStats);
                    _currentWarmEffect.OnEnter(transform);
                }
            }
            else if (other.CompareTag("RadioactiveEnvironment"))
            {
                RadiationEffect radiationEffect = other.GetComponent<RadiationEffect>();
                if (radiationEffect != null)
                {
                    _currentRadiationEffect = radiationEffect;
                    _currentRadiationEffect.SetCharacterStats(_characterStats);
                    _currentRadiationEffect.OnEnter(_resist.RadiationResist);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_currentWarmEffect != null && _currentWarmEffect.MatchesTrigger(other))
            {
                _currentWarmEffect.OnExit(transform);

                // Check if the player previously entered a cold zone
                if (_currentColdEffect != null)
                {
                    Debug.Log("Exited Warm Zone, returning to Cold Zone");
                    _currentColdEffect.SetCharacterStats(_characterStats);
                    _currentColdEffect.OnEnter(transform, _resist.ColdResist);
                }

                _currentWarmEffect = null;
            }
            else if (_currentColdEffect != null && _currentColdEffect.MatchesTrigger(other))
            {
                _currentColdEffect.OnExit(transform, _resist.ColdResist);
            }
            else if (_currentRadiationEffect != null && _currentRadiationEffect.MatchesTrigger(other))
            {
                _currentRadiationEffect.OnExit(_resist.RadiationResist);
                _currentRadiationEffect = null;
            }
        }
    }
}
