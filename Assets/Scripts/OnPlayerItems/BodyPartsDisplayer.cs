using System.Collections.Generic;
using UnityEngine;

namespace ArmorSystem
{
    public class BodyPartsDisplayer : MonoBehaviour
    {
        public static BodyPartsDisplayer singleton { get; private set; }

        [SerializeField] private List<BodyPart> _bodyParts = new List<BodyPart>();

        private void Start()
        {
            singleton = this;
            CacheParts();
        }

        private void CacheParts()
        {
            foreach(var bodyPart in _bodyParts)
                bodyPart.CacheRenderers();
        }
        
        public void DressArmor(BodyPartType bodyPartType, Material material)
        {
            foreach (var bodyPart in _bodyParts)
            {
                if(bodyPart.BodyPartType != bodyPartType) continue;
                bodyPart.AssignMaterial(material);
            }
        }
    }
}