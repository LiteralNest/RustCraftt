using Inventory_System.Inventory_Items_Displayer;
using Items_System.Items;
using Storage_System;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory_System.Inventory_Slot_Displayers
{
    public class DamagableItemDisplayer : InventoryItemDisplayer
    {
        [SerializeField] private Image _hpBar;
        [SerializeField] private GameObject _hpBarParent;
        private DamagableItem _currentItem;

        public override void DisplayData()
        {
            if (InventoryCell.Item == null) return;
            var item = InventoryCell.Item as DamagableItem;
            _currentItem = item;
        
            if (InventoryCell.Hp <= 0)
            {
                InventoryCell.Hp = item.Hp;
                if (PreviousCell != null)
                {
                    InventoryHandler.singleton.CharacterInventory.SetItemServerRpc(PreviousCell.Index,
                        new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count,
                            InventoryCell.Hp, InventoryCell.Ammo));
                }
                DisplayBar(InventoryCell.Hp);
            }
            DisplayBar(InventoryCell.Hp);
        }

        public void DisplayBar(int hp)
        {
            if(_currentItem.Hp <= 0)
            {
                _hpBarParent.SetActive(false);
                return;
            }
            _hpBar.fillAmount = (float)hp / _currentItem.Hp;
        }

        public override void MinusCurrentHp(int hp)
        {
            var item = InventoryCell.Item as Tool;
            if(item.Hp <= 0) return;
            InventoryCell.Hp -= hp;

            if (InventoryCell.Hp <= 0)
            {
                if(InventoryHandler.singleton.ActiveSlotDisplayer.Index == PreviousCell.Index)
                    GlobalEventsContainer.OnCurrentItemDeleted?.Invoke();
                InventoryHandler.singleton.CharacterInventory.RemoveItemCountFromSlotServerRpc(PreviousCell.Index, InventoryCell.Item.Id, InventoryCell.Count);
                return;
            }

            InventoryHandler.singleton.CharacterInventory.SetItemServerRpc(PreviousCell.Index,
                new CustomSendingInventoryDataCell(InventoryCell.Item.Id, InventoryCell.Count,
                    InventoryCell.Hp, InventoryCell.Ammo));
            DisplayBar(InventoryCell.Hp);
        }

        public override int GetCurrentHp()
            => InventoryCell.Hp;
        
    }
}