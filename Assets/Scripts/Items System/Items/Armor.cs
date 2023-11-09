using ArmorSystem.Backend;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Armor")]
public class Armor : CraftingItem
{
    [Header("Armor")]
    [SerializeField] private BodyPartType _bodyPartType;
    public BodyPartType BodyPartType => _bodyPartType;
}