using System.Collections.Generic;
using UnityEngine;

namespace ArmorSystem.Backend
{
    public class DressedArmorsHandler : MonoBehaviour
    {
        [SerializeField] private List<DressedArmorSlot> _slots = new List<DressedArmorSlot>();

        private DressedArmorSlot GetDressedSlotByPart(BodyPartType partType)
        {
           foreach(var slot in _slots)
               if(slot.BodyPartType == partType) 
                   return slot;
           Debug.LogError("Can't find slot");
           return new DressedArmorSlot();
        }

        public void DressArmor(BodyPartType bodyPartType, Armor armor)
        {
            if (bodyPartType == BodyPartType.All)
            {
                foreach (var slot in _slots)
                    slot.Armor = armor;
                return;
            }
            var dressedSlot= GetDressedSlotByPart(bodyPartType);
            dressedSlot.Armor = armor;
        }
    }
}