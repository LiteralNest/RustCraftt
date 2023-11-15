using ArmorSystem.Backend;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Armor")]
public class Armor : DamagableItem
{
    [Header("Armor")]
    [SerializeField] private BodyPartType _bodyPartType;
    public BodyPartType BodyPartType => _bodyPartType;
}