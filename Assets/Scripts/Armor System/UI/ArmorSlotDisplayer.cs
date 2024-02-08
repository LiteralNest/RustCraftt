using System.Collections.Generic;
using Armor_System.BackEnd.Body_Part;
using Inventory_System;
using Inventory_System.Inventory_Items_Displayer;
using Inventory_System.Inventory_Slot_Displayers;
using Items_System.Items;
using OnPlayerItems;
using UnityEngine;

namespace Armor_System.UI
{
    public class ArmorSlotDisplayer : InventorySlotDisplayer
    {
        [SerializeField] private ArmorsContainer _armorsContainer;
        [SerializeField] private List<BodyPartType> _bodyPartTypes;
        private Armor _defaultArmor;
        private Armor _currentArmor;

        private void Start()
            => _defaultArmor = _currentArmor;

        public Armor GetCurrentArmor()
        {
            if(_currentArmor == _defaultArmor) return null;
            return _currentArmor;
        }

        protected override bool TrySetItem(ItemDisplayer itemDisplayer)
        {
            if (!(itemDisplayer.InventoryCell.Item is Armor armor && _bodyPartTypes.Contains(armor.BodyPartType)))
                return false;

            _currentArmor = armor;
            InventoryHandler.singleton.ArmorsContainer.AssignItem(armor.Id);
           

            return base.TrySetItem(itemDisplayer);
        }

        public override void ResetItemWhileDrag()
        {
            base.ResetItemWhileDrag();
            if (_currentArmor != null)
                _armorsContainer.PutOffItem(_currentArmor.Id);
            _currentArmor = _defaultArmor;
        }
    }
}