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
        public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler)
        {
            base.Click(slotDisplayer, handler);
            handler.Stats.PlusStat(_statType, _addingValue);
            InventoryHandler.singleton.CharacterInventory.RemoveItem(Id, 1);
            PlayerNetCode.Singleton.PlayerSoundsPlayer.PlayHit(_eatingSound);
        }
    }
}