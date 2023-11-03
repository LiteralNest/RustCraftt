using UnityEngine;

namespace ArmorSystem
{
    public struct CachedBodyRenderer
    {
        public Renderer TargetRenderer { get; set; }
        public Material DefaultMaterial { get; set; }
    }
}