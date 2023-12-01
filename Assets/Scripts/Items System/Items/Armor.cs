using ArmorSystem.Backend;
using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/Armor")]
    public class Armor : DamagableItem
    {
        [Header("Armor")]
        [SerializeField] private BodyPartType _bodyPartType;
        public BodyPartType BodyPartType => _bodyPartType;
    }
}