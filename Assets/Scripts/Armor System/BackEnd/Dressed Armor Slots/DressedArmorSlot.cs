using Armor_System.BackEnd.Body_Part;
using Items_System.Items;
using UnityEngine;

namespace Armor_System.BackEnd.Dressed_Armor_Slots
{
    [System.Serializable]
    public class DressedArmorSlot
    {
        [field: SerializeField] public BodyPartType BodyPartType { get; set; }
        [field: SerializeField] public Armor Armor { get; set; }
    }
}