using System.Collections.Generic;
using ArmorSystem.Backend;
using UnityEngine;

namespace ArmorSystem.UI
{
    public class ArmorSlotDisplayer : InventorySlotDisplayer
    {
        [SerializeField] private List<BodyPartType> _bodyPartTypes;

        protected override bool TrySetItem(ItemDisplayer itemDisplayer)
        {
            if (itemDisplayer.InventoryCell.Item is Armor armor && _bodyPartTypes.Contains(armor.BodyPartType))
            {
                InventoryHandler.singleton.ArmorsContainer.DisplayArmor(armor);
            }

            base.TrySetItem(itemDisplayer);
            return false;
        }
    }

}