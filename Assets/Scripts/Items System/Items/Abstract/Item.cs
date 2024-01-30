using Events;
using Inventory_System.Inventory_Slot_Displayers;
using UnityEngine;

namespace Items_System.Items.Abstract
{
    public abstract class Item : ScriptableObject
    {
        [field: SerializeField] public int Id;
        [field: SerializeField] public string Name;
        [field: SerializeField] public Sprite Icon;
        [TextArea] [field: SerializeField] public string Description;
        [field: SerializeField] public int StackCount = 1000;

        public virtual void Click(SlotDisplayer quickSlotDisplayer)
        {
            var handler = InventoryHandler.singleton;
            GlobalEventsContainer.ShouldDisplayHandItem?.Invoke(-1,
                handler.PlayerNetCode.GetClientId());
            handler.ActiveSlotDisplayer = quickSlotDisplayer;
        }

        public virtual void OnClickDisabled()
        {
        }
    }
}