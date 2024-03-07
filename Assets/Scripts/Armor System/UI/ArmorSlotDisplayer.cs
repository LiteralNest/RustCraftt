using System.Collections.Generic;
using Armor_System.BackEnd.Body_Part;
using Inventory_System;
using Inventory_System.Inventory_Items_Displayer;
using Inventory_System.Inventory_Slot_Displayers;
using Items_System.Items;
using OnPlayerItems;
using Player_Controller;
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
            else
                _armorsContainer.CheckFullDressArmor();
            
            TryResetArmor();
            _currentArmor = _defaultArmor;
            _currentArmor = armor;
            InventoryHandler.singleton.ArmorsContainer.AssignItem(armor.Id);
            ArmorSystemEventsContainer.ArmorSlotDataChanged?.Invoke();
            _armorsContainer.DisplayResistValues();
            return base.TrySetItem(itemDisplayer);
        }

        public void ResetInventoryArmor()
        {
            if (ItemDisplayer == null || _currentArmor == null || _currentArmor == _defaultArmor) return;

            var cachedIndex = ItemDisplayer.PreviousCell.Index;
            var cachedInventoryCell = new InventoryCell(ItemDisplayer.InventoryCell);

            Inventory.RemoveItemCountFromSlotServerRpc(cachedIndex,
                cachedInventoryCell.Item.Id, 1);
            Inventory.AddItemToDesiredSlotServerRpc(cachedInventoryCell.Item.Id, 1, cachedInventoryCell.Ammo,
                cachedInventoryCell.Hp);
            TryResetArmor();
            _currentArmor = _defaultArmor;
            _armorsContainer.DisplayResistValues();
        }

        public void CheckForFullDress()
        {
            if (ItemDisplayer == null || _currentArmor == null || _currentArmor == _defaultArmor) return;
            if (_currentArmor.BodyPartType != BodyPartType.All) return;
            ResetInventoryArmor();
        }

        private void TryResetArmor()
        {
            if (_currentArmor == null) return;
            _armorsContainer.PutOffItem(_currentArmor.Id, PlayerNetCode.Singleton);
            ArmorSystemEventsContainer.ArmorSlotDataChanged?.Invoke();
        }

        public override void ResetItemWhileDrag()
        {
            ArmorSystemEventsContainer.ArmorSlotDataChanged?.Invoke();
            TryResetArmor();
            _currentArmor = _defaultArmor;
            _armorsContainer.DisplayResistValues();
            base.ResetItemWhileDrag();
        }
    }
}