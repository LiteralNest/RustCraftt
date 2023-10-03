using UnityEngine;

namespace SurroundingEffectsSystem
{
    public class EnvironmentZoneBehaviour : MonoBehaviour
    {
        private SurroundingEffectsStateType currentEffect;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("EnvironmentZone"))
            {
                TemperatureZone temperatureZone = other.GetComponent<TemperatureZone>();
                if (temperatureZone != null)
                {
                    SetEffect(temperatureZone.EffectType);
                    Debug.Log(temperatureZone.EffectType);
                }
            }
        }

        private void SetEffect(SurroundingEffectsStateType effect)
        {
            if (currentEffect != effect)
            {
                currentEffect = effect;
                
                GlobalEventsContainer.TemperatureChanged?.Invoke(effect);

                if (effect == SurroundingEffectsStateType.Critical)
                {
                    GlobalEventsContainer.CriticalTemperatureReached?.Invoke();
                }
                else if (effect == SurroundingEffectsStateType.Radiation)
                {
                    GlobalEventsContainer.RadiationStarted?.Invoke();
                }
                else if (currentEffect != SurroundingEffectsStateType.Radiation)
                {
                    GlobalEventsContainer.RadiationEnded?.Invoke();
                }
            }
        }
    }
}