using System.Collections.Generic;
using Inventory_System.Inventory_Slot_Displayers;
using Player_Controller;
using UnityEngine;

namespace Items_System.Items.Food
{
    [CreateAssetMenu(menuName = "Item/Food")]
    public class Food : Resource
    {
        [Header("Character Stat Riser")]
        [SerializeField] private List<FoodSlot> _slots;
        [SerializeField] private AudioClip _eatingSound;
        public override void Click(SlotDisplayer slotDisplayer)
        {
            base.Click(slotDisplayer);
            foreach (var slot in _slots)
                InventoryHandler.singleton.Stats.PlusStat(slot.StatType, slot.AddingValue);
           
            slotDisplayer.Inventory.RemoveItemCountFromSlotServerRpc(slotDisplayer.Index, Id, 1);
            PlayerNetCode.Singleton.PlayerSoundsPlayer.PlayHit(_eatingSound);
        }
    }
}