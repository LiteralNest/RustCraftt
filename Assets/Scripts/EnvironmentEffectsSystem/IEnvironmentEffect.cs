using UnityEngine;

namespace SurroundingEffectsSystem
{
    public interface IEnvironmentEffect
    {
        void OnEnter();
        void OnExit();
        bool MatchesTrigger(Collider other);
    }
}