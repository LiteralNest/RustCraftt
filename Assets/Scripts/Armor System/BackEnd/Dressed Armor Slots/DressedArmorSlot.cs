using Items_System.Items;
using UnityEngine;

namespace ArmorSystem.Backend
{
    [System.Serializable]
    public class DressedArmorSlot
    {
        [field: SerializeField] public BodyPartType BodyPartType { get; set; }
        [field: SerializeField] public Armor Armor { get; set; }
    }
}