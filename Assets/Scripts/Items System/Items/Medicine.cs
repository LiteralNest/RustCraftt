using Inventory_System.Inventory_Slot_Displayers;
using Items_System.Items.Abstract;
using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/Medicine")]
    public class Medicine : CraftingItem
    {
        [Header("Medicine")]
        [SerializeField] private int _addingValue;
    
        public override void Click(SlotDisplayer slotDisplayer)
        {
            base.Click(slotDisplayer);
            InventoryHandler.singleton.Stats.PlusStat(CharacterStatType.Health, _addingValue);
            InventoryHandler.singleton.CharacterInventory.RemoveItemCountFromSlotServerRpc(slotDisplayer.Index, Id, 1);
        }
    }
}
