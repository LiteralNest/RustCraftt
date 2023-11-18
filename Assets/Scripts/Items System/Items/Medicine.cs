using UnityEngine;

[CreateAssetMenu(menuName = "Item/Medicine")]
public class Medicine : CraftingItem
{
    [Header("Medicine")]
    [SerializeField] private int _addingValue;
    
    public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler)
    {
        base.Click(slotDisplayer, handler);
        handler.Stats.PlusStat(CharacterStatType.Health, _addingValue);
        InventoryHandler.singleton.CharacterInventory.RemoveItemCountServerRpc(Id, 1);
    }
}
