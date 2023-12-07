using Inventory_System.Inventory_Slot_Displayers;
using Storage_System;

namespace Inventory_System.Inventory_Items_Displayer
{
    public class LongRangeWeaponItemDisplayer : DamagableItemDisplayer
    {
        public override void MinusCurrentAmmo(int value)
        {
            InventoryCell.Ammo -= value;
            InventoryHandler.singleton.CharacterInventory.SetItemServerRpc(PreviousCell.Index,
                new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count,
                    InventoryCell.Hp, InventoryCell.Ammo));
            GlobalEventsContainer.InventoryDataChanged?.Invoke();
        }

        public override void SetCurrentAmmo(int value)
        {
            InventoryCell.Ammo = value;
            InventoryHandler.singleton.CharacterInventory.SetItemServerRpc(PreviousCell.Index,
                new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count,
                    InventoryCell.Hp, InventoryCell.Ammo));
        }

        private void DisplayAmmo(int value)
        {
            _countText.text = value.ToString();
        }
        
        public override void DisplayData()
        {
            base.DisplayData();
            if (InventoryCell.Item == null) return;
            _itemIcon.sprite = InventoryCell.Item.Icon;
            DisplayAmmo(InventoryCell.Ammo);
        }
    }
}