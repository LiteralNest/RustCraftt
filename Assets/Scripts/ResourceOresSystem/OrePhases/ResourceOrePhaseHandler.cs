using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ResourceOresSystem.OrePhases
{
    [RequireComponent(typeof(Ore))]
    public class ResourceOrePhaseHandler : NetworkBehaviour
    {
        [SerializeField] private List<OrePhase> _phases;

        private Ore _targetOre;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _targetOre = GetComponent<Ore>();
            DisplayPhase(   _targetOre.CurrentHp.Value);
            _targetOre.CurrentHp.OnValueChanged += (int oldValue, int newValue) => { DisplayPhase(newValue); };
        }

        private void DisplayPhase(int currentHp)
        {
            foreach (var phase in _phases)
                phase.GameObject.SetActive(phase.MinHpRequired <= currentHp);
        }
    }
}