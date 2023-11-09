using UnityEngine;

[CreateAssetMenu(menuName = "Item/Food")]
public class Food : Resource
{
    [Header("Food")]
    [SerializeField] private int _addingFood;
    [SerializeField] private CharacterStatType _statType;
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler)
    {
        base.Click(slotDisplayer, handler);
        handler.Stats.PlusStat(_statType, _addingFood);
        InventorySlotsContainer.singleton.RemoveItemFromDesiredSlot(this, 1);
    }
}