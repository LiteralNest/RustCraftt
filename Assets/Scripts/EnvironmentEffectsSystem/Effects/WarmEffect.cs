using System.Collections;
using SurroundingEffectsSystem;
using UnityEngine;

namespace EnvironmentEffectsSystem.Effects
{
    public class WarmEffect //: IEnvironmentEffect
    {
        private CharacterStats _characterStats;

        public WarmEffect(CharacterStats characterStats)
        {
            _characterStats = characterStats;
        }

        public bool MatchesTrigger(Collider other)
        {
            return other.CompareTag("WarmEnvironment");
        }

        public void OnEnter()
        {
           
        }

        public void OnExit()
        {
            
        }

        public IEnumerator EffectByTime(Collider collider)
        {
            throw new System.NotImplementedException();
        }
        
        public IEnumerator DecreaseEffectOverTime()
        {
            yield return null;
        }
    }
}