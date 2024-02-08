using Events;
using Inventory_System;
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

        public int AddingValue => _addingValue;
    
        public override void Click(SlotDisplayer slotDisplayer)
        {
            base.Click(slotDisplayer);
            var handler = InventoryHandler.singleton;
            GlobalEventsContainer.ShouldDisplayHandItem?.Invoke(slotDisplayer.ItemDisplayer.InventoryCell.Item.Id,
                handler.PlayerNetCode.GetClientId());
            handler.SetActiveItem(this);
        }
    }
}
