using Inventory_System.Inventory_Slot_Displayers;
using Player_Controller;
using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/CharacterStatRiser")]
    public class Food : Resource
    {
        [Header("Character Stat Riser")]
        [SerializeField] private int _addingValue;
        [SerializeField] private CharacterStatType _statType;
        [SerializeField] private AudioClip _eatingSound;
        public override void Click(SlotDisplayer slotDisplayer)
        {
            base.Click(slotDisplayer);
            InventoryHandler.singleton.Stats.PlusStat(_statType, _addingValue);
            InventoryHandler.singleton.CharacterInventory.RemoveItemCountFromSlotServerRpc(slotDisplayer.Index, Id, 1);
            PlayerNetCode.Singleton.PlayerSoundsPlayer.PlayHit(_eatingSound);
        }
    }
}