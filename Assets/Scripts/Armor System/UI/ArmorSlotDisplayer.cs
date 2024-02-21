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
            if (_currentArmor == _defaultArmor) return null;
            return _currentArmor;
        }

        protected override bool TrySetItem(ItemDisplayer itemDisplayer)
        {
            if (!(itemDisplayer.InventoryCell.Item is Armor armor && _bodyPartTypes.Contains(armor.BodyPartType)))
                return false;

            if (armor.BodyPartType == BodyPartType.All)
                _armorsContainer.ResetItems();

            TryResetArmor();
            _currentArmor = armor;
            InventoryHandler.singleton.ArmorsContainer.AssignItem(armor.Id);

            ArmorSystemEventsContainer.ArmorSlotDataChanged?.Invoke();
            return base.TrySetItem(itemDisplayer);
        }

        public void ResetInventoryArmor()
        {
            if (ItemDisplayer == null || _currentArmor == null || _currentArmor == _defaultArmor) return;
            //sInventory.AddItemToDesiredSlot(_currentArmor.Id, 1, ItemDisplayer.InventoryCell.Hp, 0);
            TryResetArmor();
            _currentArmor = _defaultArmor;
            Destroy(ItemDisplayer.gameObject);
            ResetItem();
        }

        private void TryResetArmor()
        {
            if (_currentArmor == null) return;
            _armorsContainer.PutOffItem(_currentArmor.Id);
            ArmorSystemEventsContainer.ArmorSlotDataChanged?.Invoke();
        }

        public override void ResetItemWhileDrag()
        {
            ArmorSystemEventsContainer.ArmorSlotDataChanged?.Invoke();
            TryResetArmor();
            _currentArmor = _defaultArmor;
            base.ResetItemWhileDrag();
        }
    }
}