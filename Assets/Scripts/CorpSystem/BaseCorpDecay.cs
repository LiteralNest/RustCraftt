using System.Collections;
using ResourceOresSystem;
using Unity.Netcode;
using UnityEngine;

namespace CorpSystem
{
    public abstract class BaseCorpDecay : NetworkBehaviour
    {
        [Header("Base")] [SerializeField] private CorpOre _targetCorpOre;

        protected IEnumerator StartDecayRoutine(int rotSecondsTime)
        {
            if (!IsServer) yield break;
            float waitingTime = _targetCorpOre.CurrentHp.Value / rotSecondsTime;
            while (_targetCorpOre.CurrentHp.Value > 0)
            {
                yield return new WaitForSeconds(waitingTime);
                _targetCorpOre.MinusHpOnServer(1);
            }
        }
    }
}