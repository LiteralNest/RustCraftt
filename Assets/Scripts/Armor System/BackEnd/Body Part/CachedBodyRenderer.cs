using UnityEngine;

namespace ArmorSystem
{
    public class CachedBodyRenderer
    {
        public Renderer TargetRenderer { get; set; }
        public Material DefaultMaterial { get; set; }

        public void ReturnToDefault()
            => TargetRenderer.sharedMaterial = DefaultMaterial;
    }
}