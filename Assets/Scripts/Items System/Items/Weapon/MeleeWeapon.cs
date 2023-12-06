using UnityEngine;

namespace Items_System.Items.Weapon
{
    [CreateAssetMenu(menuName = "Item/Melee Weapon")]
    public class MeleeWeapon : Weapon
    {
        public override void Click(QuickSlotDisplayer slotDisplayer, InventoryHandler handler)
        {
            base.Click(slotDisplayer, handler);
            CharacterUIHandler.singleton.ActivateAttackButton(true);
        }
    }
}
