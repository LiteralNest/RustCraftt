using System.Collections;
using UnityEngine;

namespace SurroundingEffectsSystem
{
    public interface IEnvironmentEffect
    {
        bool MatchesTrigger(Collider other);
        void OnEnter();
        void OnExit();
        IEnumerator EffectByTime(Collider collider);
        IEnumerator DecreaseEffectOverTime();
    }
}