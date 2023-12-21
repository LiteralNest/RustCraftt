using Inventory_System.Inventory_Slot_Displayers;
using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/Tool")]
    public class Tool : DamagableItem
    {
        [field: SerializeField] public int Damage { get; set; } = 25;
        [field: SerializeField] public bool CanDamage { get; set; } = true;
        
        public override void Click(SlotDisplayer slotDisplayer)
        {
            base.Click(slotDisplayer);
            var handler = InventoryHandler.singleton;
            handler.SetActiveItem(this);
            GlobalEventsContainer.ShouldDisplayHandItem?.Invoke(slotDisplayer.ItemDisplayer.InventoryCell.Item.Id,
                handler.PlayerNetCode.GetClientId());
        }
    }
}