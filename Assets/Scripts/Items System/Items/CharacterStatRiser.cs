using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/CharacterStatRiser")]
    public class CharacterStatRiser : Resource
    {
        [Header("Character Stat Riser")]
        [SerializeField] private int _addingValue;
        [SerializeField] private CharacterStatType _statType;
        public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler)
        {
            base.Click(slotDisplayer, handler);
            handler.Stats.PlusStat(_statType, _addingValue);
            InventoryHandler.singleton.CharacterInventory.RemoveItem(Id, 1);
        }
    }
}