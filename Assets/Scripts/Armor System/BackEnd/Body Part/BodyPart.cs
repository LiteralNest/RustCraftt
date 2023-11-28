using System.Collections.Generic;
using UnityEngine;

namespace ArmorSystem.Backend
{
    [System.Serializable]
    public class BodyPart
    {
        [field: SerializeField] public BodyPartType BodyPartType { get; private set; }
        [SerializeField] private List<Renderer> _targetRenderers;
        
        private List<CachedBodyRenderer> _cachedRenderers = new List<CachedBodyRenderer>();

        public void CacheRenderers()
        {
            foreach (var renderer in _targetRenderers)
            {
                var cachedRenderer = new CachedBodyRenderer();
                cachedRenderer.TargetRenderer = renderer;
                cachedRenderer.DefaultMaterial = renderer.sharedMaterial;
                _cachedRenderers.Add(cachedRenderer);
            }
        }
        
        public void AssignMaterial(Material assigningMaterial)
        {
            foreach (var renderer in _targetRenderers)
            {
                if(renderer.sharedMaterial == null) continue;
                renderer.sharedMaterial = assigningMaterial;
            }
          
        }

        public void ReturnToDefault()
        {
            foreach (var renderer in _cachedRenderers)
                renderer.ReturnToDefault();
        }
    }
}