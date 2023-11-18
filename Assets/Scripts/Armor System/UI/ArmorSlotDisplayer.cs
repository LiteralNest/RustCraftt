using System.Collections.Generic;
using ArmorSystem.Backend;
using UnityEngine;

namespace ArmorSystem.UI
{
    public class ArmorSlotDisplayer : InventorySlotDisplayer
    {
        [SerializeField] private List<BodyPartType> _bodyPartTypes;
        private Armor _defaultArmor;
        [SerializeField] private Armor _currentArmor;

        private void Start()
            => _defaultArmor = _currentArmor;

        protected override bool TrySetItem(ItemDisplayer itemDisplayer)
        {
            if (!(itemDisplayer.InventoryCell.Item is Armor armor && _bodyPartTypes.Contains(armor.BodyPartType)))
                return false;
            
            // Доробити як будуть шапки та інша фігня
            // if (armor.BodyPartType == BodyPartType.All)
            // {
            //     var cells = Inventory.SlotsDisplayer.GetArmorSlots();
            //     foreach (var cell in cells)
            //         cell.ReturnCellToInventory();
            // }
            
            InventoryHandler.singleton.ArmorsContainer.AssignItem(armor.Id);
            _currentArmor = armor;
            
            base.TrySetItem(itemDisplayer);
            return false;
        }

        public override void ResetItemWhileDrag()
        {
            base.ResetItemWhileDrag();
            InventoryHandler.singleton.ArmorsContainer.AssignItem(_defaultArmor.Id);
            _currentArmor = _defaultArmor;
        }
    }
}