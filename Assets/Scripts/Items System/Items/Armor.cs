using ArmorSystem.Backend;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Armor")]
public class Armor : CraftingItem
{
    [Header("Armor")]
    [SerializeField] private BodyPartType _bodyPartType;
    public BodyPartType BodyPartType => _bodyPartType;
    
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
    {
        base.Click(slotDisplayer, handler, out shouldMinus);
        shouldMinus = false;
    }
}