using UnityEngine;

[CreateAssetMenu(menuName = "Item/Food")]
public class Food : Resource
{
    [SerializeField] private int _addingFood;
    
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler, out bool shouldMinus)
    {
        shouldMinus = true;
        handler.Stats.PlusStat(CharacterStatType.Food, _addingFood);
    }
}