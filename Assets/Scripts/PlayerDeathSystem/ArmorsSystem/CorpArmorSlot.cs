using System.Collections.Generic;
using UnityEngine;

namespace PlayerDeathSystem.ArmorsSystem
{
    [System.Serializable]
    public struct CorpArmorSlot
    {
        [SerializeField] private List<Renderer> _targetRenderers;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _characterMaterial;

        public void AssignMaterial()
        {
            foreach(var renderer in _targetRenderers)
                renderer.sharedMaterial = _defaultMaterial;
        }

        public void AssignCharacterMaterial()
        {
            foreach(var renderer in _targetRenderers)
                renderer.sharedMaterial = _characterMaterial;
        }
    }
}