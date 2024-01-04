using Inventory_System.Inventory_Slot_Displayers;
using UI;
using UnityEngine;

namespace Items_System.Items.Weapon
{
    [CreateAssetMenu(menuName = "Item/Melee Weapon")]
    public class MeleeWeapon : Weapon
    {
        public override void Click(SlotDisplayer slotDisplayer)
        {
            base.Click(slotDisplayer);
            CharacterUIHandler.singleton.ActivateAttackButton(true);
        }
    }
}
