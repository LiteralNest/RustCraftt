using System.Collections.Generic;
using UnityEngine;

namespace ArmorSystem.Backend
{
    public class BodyPartsDisplayer : MonoBehaviour
    {
        [SerializeField] private List<BodyPart> _bodyParts = new List<BodyPart>();

        private void Start()
            => CacheParts();

        private void CacheParts()
        {
            foreach (var bodyPart in _bodyParts)
                bodyPart.CacheRenderers();
        }

        public void DressArmor(BodyPartType bodyPartType, Material material)
        {
            foreach (var bodyPart in _bodyParts)
            {
                if (bodyPart.BodyPartType != bodyPartType) continue;
                bodyPart.AssignMaterial(material);
            }
        }
    }
}