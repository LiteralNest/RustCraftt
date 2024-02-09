using UnityEngine;
using UnityEngine.Serialization;

namespace CorpSystem
{
    public class AnimalCorpDecay : BaseCorpDecay
    {
        [Header("Animal Rotting")]
        [SerializeField] private int _decaySecondsTime = 60;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(!IsServer) return;
            StartCoroutine(StartDecayRoutine(_decaySecondsTime));
        }
    }
}