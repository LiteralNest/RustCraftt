using System.Collections.Generic;
using Armor_System.BackEnd.Body_Part;
using UnityEngine;

namespace OnPlayerItems
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
                if(material == null) continue;
                bodyPart.AssignMaterial(material);
            }
        }

        public void ReturnArmorsToDefault(BodyPartType exceptBodyPartType = BodyPartType.None)
        {
            foreach (var bodyPart in _bodyParts)
            {
                if (bodyPart.BodyPartType == exceptBodyPartType) continue;
                bodyPart.ReturnToDefault();
            }
        }
    }
}